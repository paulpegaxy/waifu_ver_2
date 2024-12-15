// Author:    -    ad
// Created: 27/07/2024  : : 2:45 AM
// DateUpdate: 27/07/2024

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Common.Extensions;
using Game.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Runtime
{
    [Factory(ApiType.Entity, true)]
    public class ApiEntity : Api<ModelApiEntity>
    {
        public async UniTask<ModelApiTempEntity> Get()
        {
            var data = await Get<ModelApiTempEntity>("/v1/chat/allCharacter", "data");
            SyncData(data);
            Data.Notification();
            return data;
        }

        private void SyncData(ModelApiTempEntity dataTemp)
        {
            Data.ExpConfigs = dataTemp.configExpRequire;
            Data.Configs = dataTemp.sortedCharacters.OrderBy(x => x.id).ToList();
            Data.MatchedChars =  Data.Configs.Where(x => x.match_status == TypeMatchGirlStatus.match).ToList();
            Data.UnMatchedChars =  Data.Configs.Where(x => x.match_status != TypeMatchGirlStatus.match).ToList();
            var data = JsonConvert.SerializeObject(Data.MatchedChars);
            GameUtils.Log("blue", $"Matched Char: {data}");
            GameUtils.Log("yellow", "Exp configs: " + JsonConvert.SerializeObject(Data.ExpConfigs));

            // Data.UnMatchedChars.ForEach(x => UnityEngine.Debug.LogError(x.id));
        }

        public async UniTask<ModelApiEntityConfig> PostAcceptGirl(int id)
        {
            return await PostSetMatch(id, true);
        }

        public async UniTask<ModelApiEntityConfig> PostDeclineGirl(int id)
        {
            return await PostSetMatch(id, false);
        }

        private async UniTask<ModelApiEntityConfig> PostSetMatch(int character_id,bool isMatch)
        {
            // await Post<ModelApiEntity>($"/v1/pets/{id}/accept", "data");
            string match_status = isMatch ? "match" : "decline";
            var status = await Post<bool>("/v1/chat/setMatch", "status", new
            {
                character_id,
                match_status
            });
            
            if (isMatch && status)
            {
                await Get();
                var entity = Data.MatchedChars.Find(x => x.id == character_id);
                if (entity == null)
                {
                    return null;
                }
                
                Data.Notification();
                
                return entity;
            }
            
            return null;
        }
    }
}