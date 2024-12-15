// Author: ad   -
// Created: 21/11/2024  : : 14:11
// DateUpdate: 21/11/2024

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Model.ChatAI;

namespace Game.Runtime
{
    [Factory(ApiType.ChatAI, true)]
    public class ApiChatAI : Api<ModelApiChatAI>
    {
        public async UniTask<List<ModelApiChatHistory>> GetChatHistory(int character_id,int page = 1,int limit = 100)
        {
            var data = await Get<List<ModelApiChatHistory>>($"/v1/chat/chatHistory/{character_id}?page={page}&&limit={limit}", "data");
            Data.UpdateDictData(character_id, data);
            return data;
        }
        
        public async UniTask<Dictionary<int,List<ModelApiChatHistory>>> FetchAllChatHistory(List<int> listId,int charIdForceSync=-1) 
        {
            foreach (var characterId in listId)
            {
                if (!Data.DictCacheChatHistory.ContainsKey(characterId))
                    await GetChatHistory(characterId);
                else
                {
                    if (charIdForceSync == characterId)
                        await GetChatHistory(characterId);
                }
            }

            return Data.DictCacheChatHistory;
        }

        public async UniTask<ModelApiChatAIReply> PostChatAI(int character_id,string message)
        {
            var data = await Post<ModelApiChatAIReply>("/v1/chat/sendMessage", "data", new
            {
                character_id,
                message
            });

            // Data = data;

            return data;
        }
        
        public async UniTask<string> GetChatAITestDirectly(int character_id,string message)
        {
            var data = await Post<string>("/v1/chat/sendMessageDirectly", "data", new
            {
                character_id,
                message
            });

            // Data = data;

            return data;
        }
    }
}