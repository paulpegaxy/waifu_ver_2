// Author: ad   -
// Created: 15/10/2024  : : 23:10
// DateUpdate: 15/10/2024

using System;
using BreakInfinity;
using Newtonsoft.Json;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUpgradePremiumChar
    {
        public string id;
        public int level;
        public int point_per_hour;
        public int waifu_id;
        public int time_to_next;
        public DateTime next_at;
        public ModelApiUpgradePremiumCharNext next;

        [JsonIgnore] public BigDouble PointProfitParse => BigDouble.Parse(point_per_hour.ToString());

        public long GetCooldownUndress()
        {
            return ServiceTime.GetTimeRemain(next_at);
        }

        public DataItemCharPremium GetCharPremium()
        {
            return DBM.Config.charPremiumConfig.GetCharData(waifu_id);
        }
    }
    
    [Serializable]
    public class ModelApiUpgradePremiumCharNext : ModelApiUpgradeCostParse
    {
        public int Point_h_add;
        public int berry_to_skip;
    }
}