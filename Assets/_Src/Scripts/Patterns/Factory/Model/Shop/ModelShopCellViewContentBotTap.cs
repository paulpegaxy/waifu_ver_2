using Template.Defines;

namespace Game.Model
{
    public class ModelShopCellViewContentBotTap : ModelShopCellView
    {
        public ModelApiShopData BotTapData;
        
        public ModelShopCellViewContentBotTap()
        {
            Type = ShopCellViewType.ContentBotTap;
        }
    }
}