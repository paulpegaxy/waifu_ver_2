using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUserData
    {
        public int id;
        public string username;
        public int user_id;
        public string name;
        public string referral_code;
        public string ref_event;
        public string user_from;
        
        public string telegram_id;
        public List<string> tags;
        public bool is_premium;
        public int current_girl_level;
        
        [JsonIgnore] public int Id => user_id > 0 ? user_id : id;

        [JsonIgnore] public string MyCurrentEvent => tags != null && tags.Count > 0 ? tags[0] : string.Empty;

        public bool IsHavePrivatePartner(string tag)
        {
            return tags.Exists(x => x.Equals(tag));
        }
    }
}