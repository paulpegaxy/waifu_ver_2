// Author:    -    ad
// Created: 28/07/2024  : : 5:23 PM
// DateUpdate: 28/07/2024

using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class BoosterItemUpgradeMultiTap : BoosterItem
    {
        protected override void OnLoadData()
        {
            txtName.text = TypeBooster.MULTI_TAP.ToBoosterName();
            txtLevel.text = $"Lv.{UpgradeInfo.current.current_level_tap + 1}";
            txtGoldNeed.text = UpgradeInfo.next.point_tap.CostParse.ToLetter();
            // imgIcon.sprite = ControllerSprite.Instance.GetBoosterIcon(TypeBooster.MULTI_TAP);
            imgIcon.LoadSpriteAutoParseAsync("booster_" + (int)TypeBooster.MULTI_TAP);
        }

        protected override void OnClickUpgrade()
        {
            this.ShowPopup(UIId.UIPopupName.PopupConfirmBoosterMultiTap);
        }
    }
}