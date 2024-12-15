using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    public class ModelShopCellViewContentTimeLapse : ModelShopCellView
    {
        public List<ModelApiShopData> RowItemData;
        
        public ModelShopCellViewContentTimeLapse()
        {
            Type = ShopCellViewType.ContentTimeLapse;
        }
    }
}