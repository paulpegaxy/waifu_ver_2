using System.Collections.Generic;

namespace Game.Model
{
    public class ModelPartnerMergePalCellViewContentBundleTimelapse : AModelPartnerMergePalCellView
    {
        public List<ModelApiShopData> ListItemData;
        
        public ModelPartnerMergePalCellViewContentBundleTimelapse()
        {
            Type = TypePartnerMergePalCellView.ContentBundleTimelapse;
        }
    }
}