using System;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class PopupConfirmBoosterRestoreEnergy : APopupConfirmBooster
    {
        protected override async void OnClickConfirm()
        {
            try
            {
                this.ShowProcessing();
                // apiGame.CheatChargeStamina();
                await apiGame.PostChargeStamina();
                await apiUpgrade.Get();
                this.GetComponent<UIPopup>().Hide();
                this.HideProcessing();
                ControllerPopup.ShowToastSuccess("Your energy is filled");
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
    }
}