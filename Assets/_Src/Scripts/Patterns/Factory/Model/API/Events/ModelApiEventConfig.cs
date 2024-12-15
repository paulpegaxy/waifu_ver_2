using System;
using System.Collections.Generic;
using Game.Runtime;
using Game.UI;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEventConfig
    {
        public string id;
        public DateTime time_start;
        public DateTime? time_end;
        public List<string> tag_private = new();
        public List<string> for_user_tags;
        public List<string> leaderboard;
        public bool empty_filter;


        public string iap_collab;
        public string iap_offer;
        public string quest_type;
        public List<string> private_modules;
        public List<string> public_modules;
        public List<string> modules_order;
        public bool claimed;
        public bool available;
        public float boost;
        public List<ModelApiItemData> items;

        public string GetPartnerTagPrivate => for_user_tags?.Count > 0 ? for_user_tags[0] : "";

        [JsonIgnore]
        public string PrivatePartnerId => id + "_private";

        [JsonIgnore] public bool IsHaveRanking => leaderboard != null && leaderboard.Contains("point");

        public bool IsStartEvent => time_start.ToUnixTimeSeconds() < ServiceTime.CurrentUnixTime;

        public bool IsHavePrivate()
        {
            return tag_private?.Count > 0;
        }

        public bool IsHaveModulePrivate(string tagPrivate)
        {
            return IsHavePrivate() && tag_private.Contains(tagPrivate);
        }

        public bool IsMatchWithUser()
        {
            var apiUser = FactoryApi.Get<ApiUser>();
            return apiUser.Data != null && apiUser.Data.User.user_from == id;
        }

        public bool IsAvailable()
        {
            return available && (IsMatchWithUser() || public_modules.Count > 0);
        }

        public bool NeedToCheckIn()
        {
            return available && IsMatchWithUser() && private_modules.Contains("Checkin");
        }

        public bool NeedToCheckEmoji()
        {
            return available && id == "emoji";
        }

        public bool IsEndEvent()
        {
            if (time_end != null)
            {
                var timeStamp = ((DateTime)time_end).ToUnixTimeSeconds();
                return timeStamp < ServiceTime.CurrentUnixTime;
            }

            return true;
        }

        public MainWindowAction GetPackageButtonType()
        {
            switch (id)
            {
                case "merge_campaign":
                    return MainWindowAction.PartnerMergePal;
                case "etaku_campaign":
                    return MainWindowAction.PartnerEtaku;
                case "waifu_pride_event":
                    return MainWindowAction.PartnerWaifuPride;
                case "yggplay_event":
                    return MainWindowAction.PartnerYggPlay;
                default:
                    return MainWindowAction.None;
            }
        }
    }
}