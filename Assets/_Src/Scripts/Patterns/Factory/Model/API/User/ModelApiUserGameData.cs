using System;
using Newtonsoft.Json;

namespace Game.Model
{
    public class ModelApiUserGameData
    {
        public ModelApiUserGameBot auto_bot;
    }

    [Serializable]
    public class ModelApiUserGameBot
    {
        public bool is_auto_bot;
        public int? trial_auto_bot_remaining_time;
        public int trial_auto_bot_limit;
        public int trial_auto_bot_used_today;
        public int trial_auto_bot_time;
        
        [JsonIgnore]
        public int TimerRemaining => trial_auto_bot_remaining_time ?? 0;

        [JsonIgnore]
        public bool IsUseBotTrial => TimerRemaining > 10;
    }
}