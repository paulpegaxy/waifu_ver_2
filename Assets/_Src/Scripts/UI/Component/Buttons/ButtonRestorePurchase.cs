using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using DG.Tweening;
using Game.Runtime;

namespace Game.UI
{
    public class ButtonRestorePurchase : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation loadAnimation;
        [SerializeField] private UIButton buttonRestore;

        private void OnEnable()
        {
            loadAnimation.DOGotoAndPause(0);
            buttonRestore.onClickEvent.AddListener(OnRestore);
        }

        private void OnDisable()
        {
            buttonRestore.onClickEvent.RemoveListener(OnRestore);
        }

        private async void OnRestore()
        {
            var apiShop = FactoryApi.Get<ApiShop>();

            buttonRestore.interactable = false;
            loadAnimation.DOPlay();

            await apiShop.CheckOrder();

            buttonRestore.interactable = true;
            loadAnimation.DOGotoAndPause(0);
        }
    }
}