using System;
using System.Collections.Generic;
using Game.Model;

namespace Game.UI
{
    [Serializable]
    public class ModelEventYukiCellViewContentBundleTimelapse : AModelEventYukiCellView
    {
        public List<ModelApiShopData> ListItemData;
        
        public ModelEventYukiCellViewContentBundleTimelapse()
        {
            Type = TypeEventYukiCellView.CotnentBundleTimelapse;
        }
    }
}