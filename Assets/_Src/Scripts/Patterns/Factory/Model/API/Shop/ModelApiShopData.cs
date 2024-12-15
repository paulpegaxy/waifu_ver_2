using System;
using System.Collections.Generic;
using ExcelDataReader.Log;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiShopData : ModelApiNotification<ModelApiShopData>
    {
        public string id;
        public string pack;
        public string type;
        public List<ModelApiItemData> items;
        public float price;
        public string currency_tele;
        public string currency_mirai;
        public bool use_ad;
        public bool use_game_item;
     
        public float sale_off_percent;
        public bool profit_buddy_tree;
        
        public float config_price;
        public List<ModelApiItemData> bonus_items;
        public int limit;
        public string buy_type;
      
        public int reset_time;
        public int purchased_count;
        
        public string config_currency;
        public float ton_price;
        public string ton_currency;

        public bool first_buy_bonus;

        public string event_id;

        public DateTime end_time;
        
        public int duration;
        public bool available;
        
        public ModelApiShopDataDetail level;
        public List<string> client_data;

        public int limit_all_user;
        public int? all_user_purchased_count;

        public List<ModelApiShopTokenData> tokens;
     
        // public TypeResource game_item_type;
  
        // public bool available;

        public ModelApiShopTokenData GetTokenStar => tokens.Find(x => x.currency.ToUpper().Equals("STAR"));
        
        public bool IsReachLimit => limit > 0 && purchased_count >= limit;

        public bool IsSoldOut => all_user_purchased_count.HasValue && all_user_purchased_count.Value >= limit_all_user;

        public bool OnChainBundle => type.Equals("onchain");

        public TypeShopItem GetItemType()
        {
            if (Enum.TryParse(id.SnakeToPascal(), out TypeShopItem type))
            {
                return type;
            }

            return TypeShopItem.None;
        }

        public TypeShopPack GetPackType()
        {
            if (Enum.TryParse(pack.SnakeToPascal(), out TypeShopPack type))
            {
                return type;
            }

            return TypeShopPack.None;
        }

        public float GetFinalPrice()
        {
            if (type.Equals("ingame"))
            {
                return Mathf.RoundToInt(price - price * (sale_off_percent != 0 ? sale_off_percent : 0));
            }
            return price - price * (sale_off_percent != 0 ? sale_off_percent : 0);
        }
        
        public bool IsReceivedBonus(string id)
        {
            return !first_buy_bonus;
        }

        public string GetLimitText()
        {
            return $"{Localization.Get(TextId.Shop_Limit)} {purchased_count}/{limit}";
        }

        public int GetPercentDiscount()
        {
            return (int)(sale_off_percent * 100);
        }

        public override void Notification()
        {
            OnChanged?.Invoke(this);
        }

        public bool IsMatchEventId(string eventId)
        {
            if (client_data?.Count > 0)
            {
                var firstData = client_data[0];
                if (firstData.Equals(eventId))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class ModelApiShopDataDetail
    {
        public int from;
        public int to;
    }
}