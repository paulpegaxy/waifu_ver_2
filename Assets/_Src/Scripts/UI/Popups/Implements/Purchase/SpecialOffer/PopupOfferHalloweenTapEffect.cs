// Author: ad   -
// Created: 04/11/2024  : : 22:11
// DateUpdate: 04/11/2024

using Game.Model;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PopupOfferHalloweenTapEffect : MonoBehaviour
    {
        [SerializeField] private ShopCellViewOfferHalloweenTap information;

        public void SetData(ModelApiShopData data)
        {
            information.SetData(new ModelShopOfferCellView()
            {
                Type = ShopOfferType.HalloweenTapEffect,
                ShopItem = data
            });
        }
    }
}