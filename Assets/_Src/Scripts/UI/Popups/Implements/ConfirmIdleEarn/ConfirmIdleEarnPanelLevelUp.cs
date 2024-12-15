using BreakInfinity;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ConfirmIdleEarnPanelLevelUp : AConfirmIdleEarnPanel
    {
        [SerializeField] private TMP_Text txtDes;
        [SerializeField] private TMP_Text txtPrice;
        [SerializeField] private ConfirmIdleEarnManageCard itemCurrent;
        [SerializeField] private ConfirmIdleEarnManageCard itemNext;
        [SerializeField] private UIButton btnLevelUp;
        
        private BigDouble _cost;
        
        private void OnEnable()
        {
            btnLevelUp.onClickEvent.AddListener(OnLevelUp);
        }
        
        private void OnDisable()
        {
            btnLevelUp.onClickEvent.RemoveListener(OnLevelUp);
        }
        
        protected override void OnSetData()
        {
            string idleName = ExtensionEnum.ToIdleEarnName(Data.id);
            var highlightString = idleName.SetHighlightString(DBM.Config.visualConfig.GetColorStatus(TypeColor.GREEN_HIGHLIGHT));
            if (Data.level > 0)
                txtDes.text = string.Format(Localization.Get(TextId.Idleearn_AskLvUp), highlightString);
            else
                txtDes.text = string.Format(Localization.Get(TextId.Idleearn_AskUnlock), highlightString);
            _cost = Data.cost;
            txtPrice.text = _cost.ToLetter();
            txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, _cost);

            SpecialExtensionGame.SetDataCardConfirmIdleEarn(ref itemCurrent, Data.id, Data.level, Data.profitPerHour);
            SpecialExtensionGame.SetDataCardConfirmIdleEarn(ref itemNext, Data.id, Data.level + 1, Data.profitAfter);
        }

        private async void OnLevelUp()
        {
            int next = Data.level + 1;
            if (!ControllerResource.IsEnough(Data.typeCurrency, _cost))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughSc));
                return;
            }
            
            var apiUpgrade = FactoryApi.Get<ApiUpgrade>();
            await apiUpgrade.PostUpgradeIdleEarn(Data.id);
            ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Idleearn_SuccessLvUp),
                ExtensionEnum.ToIdleEarnName(Data.id), next));

            await FactoryApi.Get<ApiGame>().GetInfo();
        }
    }
}