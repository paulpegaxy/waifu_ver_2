using Template.Defines;

namespace Game.Model
{
    public class ModelShopCellViewContentSubscription : ModelShopCellView
    {
        public ModelApiSubscriptionConfig Config;
        public int Index;
        
        public ModelShopCellViewContentSubscription()
        {
            Type = ShopCellViewType.ContentSubscription;
        }
    }
}