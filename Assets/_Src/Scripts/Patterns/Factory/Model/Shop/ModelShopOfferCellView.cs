using System;
using Game.UI;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelShopOfferCellView : IESModel<ShopOfferType>
    {
        public ShopOfferType Type { get; set; }
        public ModelApiShopData ShopItem;
    }
}