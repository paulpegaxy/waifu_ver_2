// Author: ad   -
// Created: 14/12/2024  : : 17:12
// DateUpdate: 14/12/2024

using Doozy.Runtime.UIManager.Components;
using Game.Defines;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UserProfileCellViewZodiac : AUserProfileCellView
    {
        [SerializeField] private Image imgIconZodiac;
        
        protected override void OnLoadData(ModelApiChatInfoExtra data)
        {
            var split = data.zodiac.Split('_');
            var index = int.Parse(split[^1]);
            var type = (TypeZodiac) index;
            imgIconZodiac.sprite = ControllerSprite.Instance.GetZodiacIcon(type);
        }
    }
}