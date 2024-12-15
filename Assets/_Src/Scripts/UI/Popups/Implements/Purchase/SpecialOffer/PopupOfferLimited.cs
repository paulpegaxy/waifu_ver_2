// Author: ad   -
// Created: 04/11/2024  : : 22:11
// DateUpdate: 04/11/2024

using Game.Model;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PopupOfferLimited : MonoBehaviour
    {
        [SerializeField] private ShopCellViewOfferLimited information;

        public void SetData(ModelApiShopData data)
        {
            information.SetData(new ModelShopOfferCellView()
            {
                Type = ShopOfferType.Limited,
                ShopItem = data
            });
        }
    }
}