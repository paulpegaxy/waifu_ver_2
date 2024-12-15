// Author: ad   -
// Created: 21/11/2024  : : 14:11
// DateUpdate: 21/11/2024

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model.ChatAI
{
    [Serializable]
    public class ModelApiChatAI : ModelApiNotification<ModelApiChatAI>
    {
        public Dictionary<int, List<ModelApiChatHistory>> DictCacheChatHistory = new();
        
        public override void Notification()
        {
            OnChanged?.Invoke(this);
        }
        
        public List<ModelApiChatHistory> GetChatHistory(int characterId)
        {
            if (DictCacheChatHistory == null)
            {
                DictCacheChatHistory = new Dictionary<int, List<ModelApiChatHistory>>();
            }

            var listData = DictCacheChatHistory.GetValueOrDefault(characterId);
            listData = listData.OrderBy(x => x.id).ToList();
            return listData;
        }

        public void UpdateDictData(int characterId, List<ModelApiChatHistory> listData)
        {
            if (DictCacheChatHistory == null)
            {
                DictCacheChatHistory = new Dictionary<int, List<ModelApiChatHistory>>();
            }

            if (DictCacheChatHistory.ContainsKey(characterId))
            {
                DictCacheChatHistory[characterId] = listData;
            }
            else
            {
                DictCacheChatHistory.Add(characterId, listData);
            }
        }
    }
}