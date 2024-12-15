using System;
using Game.Model;

namespace Game.UI
{
    [Serializable]
    public class ModelEventYukiCellViewContentBundleBotTap : AModelEventYukiCellView
    {
        public ModelApiShopData Data;
        
        public ModelEventYukiCellViewContentBundleBotTap()
        {
            Type = TypeEventYukiCellView.ContentBundleBotTap;
        }
    }
}