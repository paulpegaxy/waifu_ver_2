// Author: ad   -
// Created: 15/09/2024  : : 23:09
// DateUpdate: 15/09/2024

using System;
using System.Collections.Generic;
using Game.UI;
using Newtonsoft.Json;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEvent : ModelApiNotification<ModelApiEvent>
    {
        public List<ModelApiEventConfig> events;
        public List<ModelApiEventConfig> Configs;
        public List<ModelApiEventCheckin> Checkins = new();
        public ModelApiEventJackpot Jackpot;
        public List<ModelApiEventJackpotMyHistory> JackpotHistories;

        public ModelApiEventData event_data;

        [JsonIgnore] public ModelApiEventConfig EventMeetYuki;

        [JsonIgnore] public ModelApiEventConfig EventBundleOffer;

        public Dictionary<MainWindowAction, ModelApiEventConfig> _dictPartnerData;
 
        public ModelApiEventConfig GetEventByType(MainWindowAction type)
        {
            // return _listPartnerData.Find(x => type == x.GetPackageButtonType());
            return _dictPartnerData.GetValueOrDefault(type);
        }

        public List<ModelApiEventConfig> GetConfigs()
        {
            if (Configs == null)
            {
                return new List<ModelApiEventConfig>();
            }

            return Configs.FindAll(x => x.IsAvailable());
        }

        public ModelApiEventConfig GetConfig(string id)
        {
            return Configs.Find(x => x.id == id);
        }

        public ModelApiEventCheckin GetCheckin(string id)
        {
            return Checkins.Find(x => x.id == id);
        }

        public bool IsEndEventMeetYuki()
        {
            if (EventMeetYuki != null)
            {
                return SpecialExtensionGame.CanEndEventMeetYuki();
            }

            return event_data.background_yuki_claimed >= GameConsts.MAX_NUMBER_BG_EVENT_YUKI;
        }

        public override void Notification()
        {
            if (events.Count <= 0)
                return;

            ProcessSync();

            OnChanged?.Invoke(this);
        }

        private void ProcessSync()
        {
            string eventMeetYuki = "meet_yuki_first50k";
            string eventBundleOffer = "halloween_event";
            _dictPartnerData = new Dictionary<MainWindowAction, ModelApiEventConfig>();

            for (int i = 0; i < events.Count; i++)
            {
                var ele = events[i];
                // ele.tag_private.Add("etaku");
                
                if (!ele.IsStartEvent)
                {
                    continue;
                }

                if (ele.id == eventMeetYuki)
                {
                    EventMeetYuki = events[i];
                }else if (ele.id == eventBundleOffer)
                {
                    EventBundleOffer = events[i];
                }
                else
                {
                    //test
                    // if (ele.id == "waifu_pride_event")
                    // {
                    //     ele.empty_filter = true;
                    // }
                    
                    _dictPartnerData.Add(ele.GetPackageButtonType(), ele);
                }
            }
        }
    }

    [Serializable]
    public class ModelApiEventData
    {
        public int background_yuki_claimed;
    }
}