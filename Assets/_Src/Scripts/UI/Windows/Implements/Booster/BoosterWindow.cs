
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.UI
{
    public class BoosterWindow : UIWindow
    {
        [SerializeField] private UIButton btnGuide;
        [SerializeField] private BoosterItemChargeStamina itemChargeStamina;
        [SerializeField] private BoosterItem[] arrItemUpgrade;
        [SerializeField] private CanvasGroup canvasContent;

        protected override void OnEnabled()
        {
            btnGuide.onClickEvent.AddListener(OnClickGuide);
            ModelApiUpgradeInfo.OnChanged += OnModelApiUpgradeInfoChanged;
            InitShow();
        }

        protected override void OnDisabled()
        {
            btnGuide.onClickEvent.RemoveListener(OnClickGuide);
            ModelApiUpgradeInfo.OnChanged -= OnModelApiUpgradeInfoChanged;
        }

        private async void InitShow()
        {
            try
            {
                this.ShowProcessing();
                canvasContent.DOFade(0, 0);
                await FactoryApi.Get<ApiUpgrade>().Get();
                canvasContent.DOFade(1, 0);
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        private void ReloadData()
        {
            itemChargeStamina.LoadData();
            arrItemUpgrade.ForEach(x => x.LoadData());
        }

        private void OnClickGuide()
        {
            ControllerPopup.ShowToastComingSoon();
        }
        
        private void OnModelApiUpgradeInfoChanged(object data)
        {
           ReloadData();
        }
    }
}
