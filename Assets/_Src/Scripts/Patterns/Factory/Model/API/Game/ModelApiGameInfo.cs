using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using BreakInfinity;
using DG.DemiLib;
using Game.Debug;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiGameInfo : ModelApiNotification<ModelApiGameInfo>
    {
        public string point;
        public string point_per_tap;
        public float point_per_second;

        public int rate_tap_bonus;

        public string stamina; //stamina hiện tại
        public int stamina_max; //stamina max
        public int stamina_per_second;

        public string berry;
        public float Ton;

        public string point_all_time;
        public string berry_all_time;

        public int highest_level;

        public int speed_up_rate;

        public ModelApiGameIdleEarning idle_earnings;

        public int current_level_girl;
        public ModelApiGameNextGirl next_girl_level_data;
        public ModelApiGameInfoNextData next;
        public int current_girl_bonus;

        public string tutorial_step;

        public bool is_maintenance;


        [JsonIgnore] public BigDouble PointParse;
        [JsonIgnore] public BigDouble BerryParse;
        [JsonIgnore] public BigDouble StaminaParse;

        // [JsonIgnore] public BigDouble PointPerHourParse;
        [JsonIgnore] public int PointPerTapParse;

        [JsonIgnore] public BigDouble PointAllTimeParse;
        [JsonIgnore] public BigDouble BerryAllTimeParse;

        [JsonIgnore] public bool IsMaxVisual => (current_level_girl + 1) % GameConsts.MAX_LEVEL_PER_CHAR == 0;

        [JsonIgnore] public int CurrentGirlId;

        [JsonIgnore] public DataItemRanking DataCurrentGirlSelected;

        [JsonIgnore] public int TutorialIndex => int.TryParse(tutorial_step, out int index) ? index : 0;

        [JsonIgnore]
        public TypeLeagueCharacter CurrentCharRank =>
            (TypeLeagueCharacter)((current_level_girl / GameConsts.MAX_LEVEL_PER_CHAR) + 1);

        [JsonIgnore] public int ProfitPerHourParse;


        public bool IsNeedProtectGirl()
        {
            if (current_level_girl == 0)
                return false;

            var modValue = (current_level_girl + 1) % GameConsts.MAX_LEVEL_PER_CHAR;
            var bgConfig = DBM.Config.backgroundConfig.GetBackgroundCharNormalConfig(CurrentGirlId);
            
            if (modValue == 0)
            {
                if (!string.IsNullOrEmpty(bgConfig?.bgLevel_10))
                {
                    this.PostEvent(TypeGameEvent.ChangeBackground,true);
                    return false;
                }
            }

            if (modValue == GameConsts.MAX_LEVEL_PER_CHAR - 1)
            {
                if (!string.IsNullOrEmpty(bgConfig?.bgLevel_9))
                {
                    this.PostEvent(TypeGameEvent.ChangeBackground,true);
                    return false;
                }
            }

            return modValue == 0 || modValue == GameConsts.MAX_LEVEL_PER_CHAR - 1;
        }

        public override void Notification()
        {
            ProcessData();
            OnChanged?.Invoke(this);
        }

        public void ProcessData()
        {
            PointParse = BigDouble.Parse(point);
            BerryParse = BigDouble.Parse(berry);
            StaminaParse = BigDouble.Parse(stamina);

            PointPerTapParse = int.Parse(point_per_tap);
            PointAllTimeParse = BigDouble.Parse(point_all_time);
            BerryAllTimeParse = BigDouble.Parse(berry_all_time);

            ProcessProfitPerHour();


            var dataItemRank = DBM.Config.rankingConfig.GetDataBasedCurrentGirlLevel(current_level_girl);
            if (dataItemRank != null)
            {
                DataCurrentGirlSelected = dataItemRank;
                // PointNeedToChangeGirlSkin = dataItemRank.GetPointNeedVisualLevel(current_level_girl);
                CurrentGirlId = dataItemRank.girlId;
            }


            var storageSetting = FactoryStorage.Get<StorageSettings>();
            var model = storageSetting.Get();
            if (model != null && model.currentLevelGirl != current_level_girl)
            {
                if (model.dictLvAlreadyReadMessage.TryGetValue(CurrentGirlId, out bool isRead))
                {
                    if (isRead)
                        model.dictLvAlreadyReadMessage[CurrentGirlId] = false;
                }
                else
                {
                    model.dictLvAlreadyReadMessage.Add(CurrentGirlId, false);
                }

                model.currentLevelGirl = current_level_girl;
                storageSetting.Save();
            }

        }

        public ModelApiGameInfo Clone()
        {
            return JsonConvert.DeserializeObject<ModelApiGameInfo>(JsonConvert.SerializeObject(this));
        }

        public bool IsFresher()
        {
            return current_level_girl <= 1;
            // && PointParse == 0;
        }

        private void ProcessProfitPerHour()
        {
            var profitPerHour = point_per_second * 3600;
            var bonusFromGirl = profitPerHour * ((float)current_girl_bonus / 100);
            ProfitPerHourParse = Mathf.RoundToInt(profitPerHour + bonusFromGirl);
        }

        public int GetFinalPointPerTap()
        {
            return PointPerTapParse * rate_tap_bonus;
        }

        private bool CheckIsMaxVisualLevel()
        {
            bool isTopLevel = (current_level_girl + 1) % GameConsts.MAX_LEVEL_PER_CHAR == 0;
            if (isTopLevel)
            {
                var dataNextRank = DBM.Config.rankingConfig.GetDataBasedCurrentGirlLevel(current_level_girl + 1);
                if (dataNextRank == null)
                {
                    UnityEngine.Debug.LogError("null data Next Rank");
                    return false;
                }

                if (PointParse >= dataNextRank.totalPointNextRank)
                {
                    return true;
                }
            }

            return false;
        }
    }
}