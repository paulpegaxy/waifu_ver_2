using System;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PopupConfirmBoosterMultiTap : APopupConfirmBoosterNormal
    {
        protected override void OnInit(ModelApiUpgradeInfo dataUpgrade)
        {
            base.txtPrice.text = dataUpgrade.next.point_tap.CostParse.ToLetter();
            ProcessTextPrice(dataUpgrade.next.point_tap.CostParse);
            
            int currLevel = (dataUpgrade.current.current_level_tap + 1);
            NextLevel = currLevel + 1;
            // txtDes.text = string.Format("+" + Localization.Get(TextId.Booster_DesUpgradeMultiTap),
            //     dataUpgrade.ModPointPerTap, NextLevel);

            txtDes.text = string.Format(Localization.Get(TextId.Booster_DesUpgradeMultiTap),
                dataUpgrade.ModPointPerTap);
            
            base.manageItemCurrent.LoadData(new DataConfirmBoosterItem()
            {
                type = base.typeBooster,
                level = currLevel,
                value = dataUpgrade.current.point_per_tap
            });
            base.manageItemNext.LoadData(new DataConfirmBoosterItem()
            {
                type = base.typeBooster,
                level = NextLevel,
                value = dataUpgrade.next.point_tap.point_per_tap
            });
        }

        private void CheckTutorialActive(int tempNextLevel)
        {
            // if (SpecialExtensionGame.IsInTutorial(TutorialCategory.Main,TutorialState.MainBoosterActionUpgrade))
            if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.Booster))
                GetComponent<UIPopup>().Hide();
            else
            {
                ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Booster_UpgradeSuccess),
                        typeBooster.ToBoosterName(), tempNextLevel));
            }
        }

        protected override async void OnClickConfirm()
        {
            var currPoint = ControllerResource.Get(TypeResource.HeartPoint).Amount;
            int tempNextLevel = NextLevel;
            if (currPoint < apiUpgrade.Data.next.point_tap.cost)
            {
                ControllerPopup.ShowToastError(SpecialExtensionGame.NotiNotEnoughPoint());
                return;
            }
            try
            {
                this.ShowProcessing();

                await apiGame.PostUpgradeMultiTap();
                await apiUpgrade.Get();
                
                this.HideProcessing();

                CheckTutorialActive(tempNextLevel);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
    }
}