using Slime.UI;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ConfirmBoosterItemInfo : AConfirmBoosterItemOverlay
    {
        [SerializeField] protected TMP_Text txtLevel;
        [SerializeField] protected TMP_Text txtTitle;
        
        protected override void OnSetData()
        {
            txtLevel.text = $"Lv.{Data.level:00}";
            txtTitle.text = Data.type.ToBoosterName();
        }
    }
}