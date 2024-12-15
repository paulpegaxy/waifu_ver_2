// Author: ad   -
// Created: 22/09/2024  : : 15:09
// DateUpdate: 22/09/2024

using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PopupOfferLevelUp : MonoBehaviour
    {
        [SerializeField] private ShopCellViewOfferLevelUp information;

        public void SetData(ModelApiShopData data)
        {
            information.SetData(new ModelShopOfferCellView()
            {
                Type = ShopOfferType.LevelUp,
                ShopItem = data
            });
        }
    }
}