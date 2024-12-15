using System;
using System.Collections.Generic;
using BreakInfinity;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUpgradeInfo : ModelApiNotification<ModelApiUpgradeInfo>
    {
        public List<ModelApiIdleEarnUpgrade> upgrade;
        public ModelApiBoosterCurrent current;
        public ModelApiBoosterNext next;
        public List<ModelApiUpgradePremiumChar> premium_waifu = new();
        
        public string berry_all_time;
        public string point_all_time;

        [JsonIgnore] public int ModPointPerTap => next.point_tap.point_per_tap - current.point_per_tap;
        [JsonIgnore] public int ModEnergyMax => next.stamina_max.max_stamina - current.stamina_max;

        [JsonIgnore] public BigDouble PointAllTimeParse => BigDouble.Parse(point_all_time);
        [JsonIgnore] public BigDouble BerryAllTimeParse => BigDouble.Parse(berry_all_time);

        [JsonIgnore] public bool IsUnlockedFirstCard => upgrade?.Count > 0 && upgrade[0].current.level > 0;

        [JsonIgnore] public bool IsClaimedBgYuki => current.background.Count > 0;

        public ModelApiIdleEarnUpgrade GetCard(string cardId)
        {
            return upgrade.Find(x => x.current.id.Equals(cardId));
        }

        public ModelApiUpgradePremiumChar GetPremiumChar(int girlId)
        {
            return premium_waifu.Find(x => x.waifu_id == girlId);
        }

        public BigDouble GetTotalProfitFromPremiumChars()
        {
            BigDouble totalProfit = 0;
            foreach (var premiumChar in premium_waifu)
            {
                totalProfit += premiumChar.PointProfitParse; // Assuming `profit` is a property of `ModelApiUpgradePremiumChar`
            }

            return totalProfit;
        }
        
        public BigDouble GetPriceFirstCard()
        {
            return upgrade[0].next.CostParse;
        }

        public override void Notification()
        {
            //mockup
            // current.background.Add("50002");
            
            OnChanged?.Invoke(this);
        }
    }

}