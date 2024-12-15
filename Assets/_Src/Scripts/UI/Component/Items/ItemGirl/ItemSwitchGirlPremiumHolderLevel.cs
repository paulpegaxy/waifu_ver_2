// Author: ad   -
// Created: 17/10/2024  : : 03:10
// DateUpdate: 17/10/2024

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemSwitchGirlPremiumHolderLevel : AItemSwitchGirlPremium
    {
        [SerializeField] private Image img;
        [SerializeField] private Sprite[] arrSprite;
        

        protected override void SwitchGirl(bool isPremium)
        {
            if (isPremium)
            {
                img.sprite = arrSprite[^1];
                return;
            }
            img.sprite = arrSprite[0];
        }
    }
}