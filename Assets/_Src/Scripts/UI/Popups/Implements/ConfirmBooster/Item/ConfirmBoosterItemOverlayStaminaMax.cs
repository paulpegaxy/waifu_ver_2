using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConfirmBoosterItemOverlayStaminaMax : AConfirmBoosterItemOverlay
    {
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private Image imgIcon;
        
        protected override void OnSetData()
        {
            // var currValue = ControllerResource.Get(TypeResource.Stamina).Amount;
            // txtValue.text = $"{currValue}/{Data.value}";
            txtValue.text = $"{Data.value}/{Data.value}";
        }
    }
}