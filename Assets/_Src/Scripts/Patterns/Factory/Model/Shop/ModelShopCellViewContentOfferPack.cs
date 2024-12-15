using Template.Defines;

namespace Game.Model
{
    public class ModelShopCellViewContentOfferPack : ModelShopCellView
    {
        public ModelApiShopData OfferPackData;
        public int PackIndex;
        
        public ModelShopCellViewContentOfferPack()
        {
            Type = ShopCellViewType.ContentOfferPack;
        }
    }
}