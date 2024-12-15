using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class APopupConfirmBooster : MonoBehaviour
    {
        [SerializeField] protected TypeBooster typeBooster;
        [SerializeField] private Image imgIconBooster;
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private UIButton btnConfirm;

        protected ApiGame apiGame => FactoryApi.Get<ApiGame>();
        protected ApiUpgrade apiUpgrade => FactoryApi.Get<ApiUpgrade>();

        private void Awake()
        {
            btnConfirm.onClickEvent.AddListener(OnClickConfirm);
        }

        private void OnDestroy()
        {
            btnConfirm.onClickEvent.RemoveListener(OnClickConfirm);
        }

        private void OnEnable()
        {
            // imgIconBooster.sprite = ControllerSprite.Instance.GetBoosterIcon(typeBooster);
            imgIconBooster.LoadSpriteAutoParseAsync("booster_" + (int)typeBooster);
            // txtTitle.text = typeBooster.ToBoosterName();
            OnShow();
            ModelApiUpgradeInfo.OnChanged += OnModelApiUpgradeInfoChanged;
        }

        private void OnDisable()
        {
            ModelApiUpgradeInfo.OnChanged -= OnModelApiUpgradeInfoChanged;
        }

        protected virtual void OnShow()
        {

        }

        private void OnModelApiUpgradeInfoChanged(object data)
        {
            // UnityEngine.Debug.LogError("OnModelApiUpgradeInfoChanged Booster popup");
            OnShow();
        }

        protected abstract void OnClickConfirm();
    }
}