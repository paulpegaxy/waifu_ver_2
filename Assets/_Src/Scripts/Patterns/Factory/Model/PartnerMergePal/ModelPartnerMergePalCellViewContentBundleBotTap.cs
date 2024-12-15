// Author: ad   -
// Created: 15/09/2024  : : 01:09
// DateUpdate: 15/09/2024

using System;

namespace Game.Model
{
    [Serializable]
    public class ModelPartnerMergePalCellViewContentBundleBotTap : AModelPartnerMergePalCellView
    {
        public ModelApiShopData Data;
        
        public ModelPartnerMergePalCellViewContentBundleBotTap()
        {
            Type = TypePartnerMergePalCellView.ContentBundleBotTap;
        }
    }
}