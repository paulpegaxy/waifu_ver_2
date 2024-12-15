using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
    [Factory(ApiType.Quest, true)]
    public class ApiQuest : Api<ModelApiQuest>
    {
        public async UniTask<ModelApiQuestInfo> Get()
        {
            var info = await Get<ModelApiQuestInfo>("/v1/quests", "data");
            // var info = new 
            Sync(info);
            return info;
        }

        private void Sync(ModelApiQuestInfo info)
        {
            Data.Statistics = info.achievement_summary;
            Data.Quest = info.quests;
            // Data.Quest = info.quests.OrderByDescending(x => x.can_claim).ToList();
            
            Data.Notification();
        }
        
        public async UniTask<ModelApiQuestConfig> GetConfig()
        {
            var configs = await Get<ModelApiQuestConfig>("/v1/quests/config", "data");
            Data.Configs = configs;

            return configs;
        }

        public async UniTask<ModelApiQuestClaim> Claim(string id)
        {
            return await Post<ModelApiQuestClaim>($"/v1/quests/{id}/claim", "data", new { });
        }

        public async UniTask<ModelApiQuestCheck> Check(string id)
        {
            return await Post<ModelApiQuestCheck>($"/v1/quests/{id}/check", "data", new { });
        }
        
        public async UniTask<ModelApiQuestCheck> StoryCheck(string id)
        {
            return await Post<ModelApiQuestCheck>($"/v1/quests/{id}/story-check", "data", new { });
        }
        
        public async UniTask<ModelApiClaimComplete> EmojiCheck()
        {
            return await Post<ModelApiClaimComplete>("/v1/event/emoji/check", "data", new { });
        }
    }
}