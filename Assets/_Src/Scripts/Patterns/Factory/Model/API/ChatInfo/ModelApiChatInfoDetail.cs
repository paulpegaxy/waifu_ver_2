using System;
using System.Collections.Generic;
using Game.Extensions;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiChatInfoDetail : ModelApiNotification<ModelApiChatInfoDetail>
    {
        public int user_id;
        public int exp;
        public int expRequire;
        public int level;
        public int chat_point;
        public int swipe_count;
        public ModelApiChatInfoExtra extra_data;
        public int price_send_chat;
        public int max_swipe_count;

        public int UserLevel => level + 1;
        
        public override void Notification()
        {
            max_swipe_count = GameConsts.MAX_SWIPE_COUNT;
            OnChanged?.Invoke(this);
        }
    }
    
    [Serializable]
    public class ModelApiChatInfoExtra
    {
        public string name;
        public string interested_in;
        public string zodiac;
        public string genres;
        public string avatar;
    }
}