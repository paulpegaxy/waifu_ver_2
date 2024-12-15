// Author: ad   -
// Created: 17/10/2024  : : 03:10
// DateUpdate: 17/10/2024

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemSwitchGirlPremiumColorProfit : AItemSwitchGirlPremium
    {
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private Image imgIcon;
        
        [SerializeField] private Color[] arrColor;
        

        protected override void SwitchGirl(bool isPremium)
        {
            if (isPremium)
            {
                txtValue.color = arrColor[^1];
                imgIcon.color = arrColor[^1];
                return;
            }
            txtValue.color = arrColor[0];
            imgIcon.color = arrColor[0];
        }
    }
}