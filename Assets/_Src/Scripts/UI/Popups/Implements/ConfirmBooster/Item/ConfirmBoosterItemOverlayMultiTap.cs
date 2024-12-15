using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConfirmBoosterItemOverlayMultiTap : AConfirmBoosterItemOverlay
    {
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private Image imgIcon;
        
        protected override void OnSetData()
        {
            txtValue.text = $"+{Data.value}";
        }
    }
}