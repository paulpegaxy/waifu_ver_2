// Author: ad   -
// Created: 14/12/2024  : : 14:12
// DateUpdate: 14/12/2024

using Game.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemWaifuAvatar : MonoBehaviour
    {
        [SerializeField] private Image imgAvatar;
        [SerializeField] private Image imgHolder;
        
        public void SetAvatar(ModelApiEntityConfig entityConfig)
        {
            imgAvatar.LoadSpriteAsync(entityConfig.AvaCharKey);
            imgHolder.color = entityConfig.GetAvatarHolderColor();
        }

        public void SetAvatar(Sprite sprAva,ModelApiEntityConfig entityConfig)
        {
            imgAvatar.sprite = sprAva;
            imgHolder.color = entityConfig.GetAvatarHolderColor();
        }
    }
}