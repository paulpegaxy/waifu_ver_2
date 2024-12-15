using System;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class PopupConfirmBoosterStaminaMax : APopupConfirmBoosterNormal
    {
        protected override void OnInit(ModelApiUpgradeInfo dataUpgrade)
        {
            base.txtPrice.text = dataUpgrade.next.stamina_max.CostParse.ToLetter();
            ProcessTextPrice(dataUpgrade.next.stamina_max.CostParse);
            
            int currLevel = (dataUpgrade.current.current_level_stamina_max + 1);
            NextLevel = currLevel + 1;

            txtDes.text = $"Increase max energy to {dataUpgrade.next.stamina_max.max_stamina}";

            base.manageItemCurrent.LoadData(new DataConfirmBoosterItem()
            {
                type = base.typeBooster,
                level = currLevel,
                value = dataUpgrade.current.stamina_max
            });
            base.manageItemNext.LoadData(new DataConfirmBoosterItem()
            {
                type = base.typeBooster,
                level = NextLevel,
                value = dataUpgrade.next.stamina_max.max_stamina
            });
        }

        protected override async void OnClickConfirm()
        {
            var currPoint = ControllerResource.Get(TypeResource.HeartPoint).Amount;
            int tempNextLevel = NextLevel;
            if (currPoint < apiUpgrade.Data.next.stamina_max.cost)
            {
                ControllerPopup.ShowToastError(SpecialExtensionGame.NotiNotEnoughPoint());
                return;
            }
            try
            {
                this.ShowProcessing();
                // apiGame.CheatUpgradeMaxStamina();
                await apiGame.PostUpgradeStaminaMax();
                await apiUpgrade.Get();
                this.HideProcessing();
                ControllerPopup.ShowToastSuccess(
                    string.Format(Localization.Get(TextId.Booster_UpgradeSuccess),
                        typeBooster.ToBoosterName(), tempNextLevel));
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
    }
}