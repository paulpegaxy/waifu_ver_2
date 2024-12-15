using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class ShopOfferHalloween : ShopOffer
    {
        public override void Init(string type)
        {
            Type = ShopOfferType.HalloweenTapEffect;
            ItemType = type;
            CheckAndShow(ItemType);
        }
    }
}