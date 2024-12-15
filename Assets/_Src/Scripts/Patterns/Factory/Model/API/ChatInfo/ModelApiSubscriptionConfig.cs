// Author: ad   -
// Created: 15/12/2024  : : 14:12
// DateUpdate: 15/12/2024

using System;
using System.Collections.Generic;
using Game.UI;

namespace Game.Model
{
    [Serializable]
    public class ModelApiSubscriptionConfig
    {
        public string id;
        public bool is_featured;
        public bool is_refundable;
        public float discount_percentage;
        public bool is_auto_renewable;
        public List<ModelApiShopTokenData> price;
        public List<ModelApiItemData> item_instantly_values;
        public List<ModelApiItemData> item_daily_values;

        public int GetFirstItemValue() => item_instantly_values?.Count > 0 ? int.Parse(item_instantly_values[0].value) : 0;
        
        public int GetSecondItemValue() => item_daily_values?.Count > 0 ? int.Parse(item_daily_values[0].value) : 0;
        
        public float GetStarPrice() => price?.Count > 0 ? price[0].price : 0;
    }
}