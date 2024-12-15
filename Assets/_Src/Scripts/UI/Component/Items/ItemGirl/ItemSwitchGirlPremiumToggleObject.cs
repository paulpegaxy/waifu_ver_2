// Author: ad   -
// Created: 17/10/2024  : : 03:10
// DateUpdate: 17/10/2024

using UnityEngine;

namespace Game.UI
{
    public class ItemSwitchGirlPremiumToggleObject : AItemSwitchGirlPremium
    {
        [SerializeField] private GameObject[] arrObj;
       
        
        protected override void SwitchGirl(bool isPremium)
        {
            if (isPremium)
            {
                arrObj[0].SetActive(false);
                arrObj[1].SetActive(true);
                return;
            }
            arrObj[0].SetActive(true);
            arrObj[1].SetActive(false);
        }
    }
}