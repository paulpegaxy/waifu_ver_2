using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemBotTapPremium : MonoBehaviour
    {
        [SerializeField] private Image imgStatus;
        [SerializeField] private UIButton btnClick;
        [SerializeField] private Sprite[] arrSprStatus;

        private bool _isRunning;
        
        private void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
        }
        
        public void SetData()
        {
            gameObject.SetActive(true);
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            var model = storageSetting.Get();
            _isRunning = model.isUseBotTap;
            Refresh();
        }

        private void Refresh()
        {
            if (!_isRunning)
            {
                ControllerAutomation.Stop();
                imgStatus.sprite = arrSprStatus[0];
            }
            else
            {
                ControllerAutomation.Start(true);
                imgStatus.sprite = arrSprStatus[1];
            }
        }

        private void OnClick()
        {
            _isRunning = !_isRunning;
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            storageSetting.Get().isUseBotTap = _isRunning;
            storageSetting.Save();
            Refresh();
        }
    }
}