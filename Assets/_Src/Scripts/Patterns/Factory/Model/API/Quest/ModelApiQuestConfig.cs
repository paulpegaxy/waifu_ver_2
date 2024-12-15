using System;
using System.Collections.Generic;
using Game.Runtime;

namespace Game.Model
{
    [Serializable]
    public class ModelApiQuestConfig
    {
        public List<ModelApiQuestShareStory> share_story;

        public ModelApiQuestShareStory GetStoryVideoLinkForRanking()
        {
            return share_story.Find(x => x.quest_id == "164");
        }
        
        public ModelApiQuestShareStory GetShareStory(string questId)
        {
            var apiUser = FactoryApi.Get<ApiUser>();

            var itemFind = share_story.Find(x => x.quest_id == questId);
            if (itemFind!=null && questId != "164")
            {
                //wile remove check 164 later
                return itemFind;
            }
            
            foreach (var group in share_story)
            {
                if (apiUser.Data.User.user_id >= group.ranking_from && apiUser.Data.User.user_id <= group.ranking_to)
                {
                    return group;
                }
            }

            return share_story[0];
        }
    }

    [Serializable]
    public class ModelApiQuestShareStory
    {
        public string id;
        public string quest_id;
        public string description;
        public string link;
        public int ranking_from;
        public int ranking_to;
        public string segmentInfo;
    }

    [Serializable]
    public class ModelApiQuestRankingGroup
    {
        public string id;
        public int from;
        public int to;
        public int group;
    }
}