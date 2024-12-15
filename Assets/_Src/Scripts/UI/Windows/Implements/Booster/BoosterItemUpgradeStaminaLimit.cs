// Author:    -    ad
// Created: 28/07/2024  : : 5:25 PM
// DateUpdate: 28/07/2024

using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class BoosterItemUpgradeStaminaLimit : BoosterItem
    {
        protected override void OnLoadData()
        {
            txtName.text = TypeBooster.ENERGY_LIMIT.ToBoosterName();
            txtLevel.text = $"Lv.{UpgradeInfo.current.current_level_stamina_max + 1}";
            txtGoldNeed.text = UpgradeInfo.next.stamina_max.CostParse.ToLetter();
            imgIcon.LoadSpriteAutoParseAsync("booster_" + (int)TypeBooster.ENERGY_LIMIT);
        }

        protected override void OnClickUpgrade()
        {
            if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.Undress))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotiUpgradeNoti));
                return;
            }

            this.ShowPopup(UIId.UIPopupName.PopupConfirmBoosterStaminaMax);
        }
    }
}