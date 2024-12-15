// Author: ad   -
// Created: 08/08/2024  : : 01:08
// DateUpdate: 08/08/2024

using Game.Runtime;
using Slime.UI;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class IdleEarnCardInfo : AIdleEarnCardOverlay
    {
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtDescription;
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private TMP_Text txtLevel;
        [SerializeField] private Image imgHolderLevel;
        [SerializeField] private Image imgCardIcon;
        [SerializeField] private Image imgIconCurrency;
        [SerializeField] private Image imgIconInCard;

        protected override void OnSetData()
        {
            txtName.text = ExtensionEnum.ToIdleEarnName(Data.id);
            txtDescription.text = Localization.Get(TextId.Idleearn_ProfitPerHour);
            imgIconCurrency.sprite = ControllerSprite.Instance.GetResourceIcon(TypeResource.HeartPoint);

            txtLevel.text = $"Lv.{Data.level}";

            int id = int.Parse(Data.id);

            // imgIconInCard.sprite = ControllerSprite.Instance.GetIdleEarnIcon(id);
            imgIconInCard.LoadSpriteAutoParseAsync("idle_earn_" + id);

            var matDisable = DBM.Config.visualConfig.materialConfig.matDisableObject;

            bool isLocked = Data.level <= 0;
            imgCardIcon.material = isLocked ? matDisable : null;
            imgIconInCard.material = isLocked ? matDisable : null;
            imgHolderLevel.material = isLocked ? matDisable : null;

            if (!isLocked)
                txtValue.text = "+" + Data.profitPerHour.ToLetter();
            else
                txtValue.text = $"+{(Data.profitAfter - Data.profitPerHour).ToLetter()}";
        }
    }
}