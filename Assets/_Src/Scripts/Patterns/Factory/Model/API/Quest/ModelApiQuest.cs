using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.Utilities;

namespace Game.Model
{
    [Serializable]
    public class ModelApiQuest : ModelApiNotification<ModelApiQuest>
    {
        public ModelApiQuestStatistics Statistics;
        public ModelApiQuestConfig Configs;
        public List<ModelApiQuestData> Quest;

        private Dictionary<string, List<ModelApiQuestData>> _dictQuestEvent;
        private ModelApiQuestData _questFollowX;

        private const string EVENT_YUKI_TASK = "meet_yuki_first50k";

        public bool IsDoneFollowXQuest() => _questFollowX != null &&
                                            (_questFollowX.can_claim || _questFollowX.claimed ||
                                             _questFollowX.is_verifying);

        [JsonIgnore] public ModelApiQuestData QuestFollowXData => _questFollowX;  

        public void Sync(ModelApiQuestData data)
        {
            var quest = Quest.Find(x => x.id == data.id);
            if (quest == null) return;

            quest.process = data.process;
            quest.processed = data.processed;
            quest.can_claim = data.can_claim;
            quest.claimed = data.claimed;
            quest.end_time = data.end_time;

            Notification();
        }

        public List<ModelApiQuestData> GetListQuestEvent(string eventId)
        {
            return _dictQuestEvent.TryGetValue(eventId, out var quests) ? quests : new List<ModelApiQuestData>();
        }

        public bool IsDoneAllQuestEventYuki()
        {
            return _dictQuestEvent.TryGetValue(EVENT_YUKI_TASK, out var quests) && quests.All(x => x.claimed);
        }

        public override void Notification()
        {
            _dictQuestEvent = Quest.Where(x => !string.IsNullOrEmpty(x.event_id)).
                GroupBy(x => x.event_id).
                ToDictionary(g => g.Key, g => g.ToList());
            _questFollowX = Quest.Find(x => x.id == "141");
            
            // UnityEngine.Debug.LogError("Quest X: "+JsonConvert.SerializeObject(_questFollowX));
            
            OnChanged?.Invoke(this);
        }
    }
}