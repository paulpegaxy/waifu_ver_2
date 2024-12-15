using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    public class ModelShopCellViewContentNormal : ModelShopCellView
    {
        public List<ModelApiShopData> RowItemData;
        
        public ModelShopCellViewContentNormal()
        {
            Type = ShopCellViewType.ContentShopNormal;
        }
    }
}