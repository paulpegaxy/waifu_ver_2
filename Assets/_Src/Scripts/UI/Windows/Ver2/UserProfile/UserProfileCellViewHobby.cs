// Author: ad   -
// Created: 14/12/2024  : : 17:12
// DateUpdate: 14/12/2024

using Doozy.Runtime.UIManager.Components;
using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UserProfileCellViewHobby : AUserProfileCellView
    {
        [SerializeField] private TMP_Text txtHobby;
        [SerializeField] private Image imgIconHobby;

        protected override void OnLoadData(ModelApiChatInfoExtra data)
        {
            var index = int.Parse(data.interested_in);
            UnityEngine.Debug.Log("index: " + index);
            imgIconHobby.sprite = ControllerSprite.Instance.GetHobbyIcon(index);
            switch (index)
            {
                case 1:
                    txtHobby.text = "I'm interested in men";
                    break;
                case 2:
                    txtHobby.text = "I'm interested in women";
                    break;
                default:
                    txtHobby.text = "I'm interested in everyone";
                    break;
            }
        }
    }
}