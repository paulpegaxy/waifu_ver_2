// Author: ad   -
// Created: 17/10/2024  : : 03:10
// DateUpdate: 17/10/2024

using Game.Model;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class ItemSwitchGirlPremiumAvatar : AItemSwitchGirlPremiumCheckGameInfo
    {
        [SerializeField] private ItemAvatar itemAvatar;
        
        protected override void SwitchGirl(bool isPremium)
        {
            itemAvatar.SetImageAvatar(InitGirlId);
        }
    }
}