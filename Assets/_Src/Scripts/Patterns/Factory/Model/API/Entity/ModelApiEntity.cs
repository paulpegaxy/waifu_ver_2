using System;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Common.Extensions;
using Game.Defines;
using Game.Extensions;
using Newtonsoft.Json;
using Template.Defines;
using UnityEngine;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEntity : ModelApiNotification<ModelApiEntity>
    {
        public List<ModelApiEntityConfig> Configs;

        public List<ModelApiEntityExpConfig> ExpConfigs;
        
        [JsonIgnore]
        public List<ModelApiEntityConfig> UnMatchedChars;
        
        [JsonIgnore]
        public List<ModelApiEntityConfig> MatchedChars;

        private int _index;

        public override void Notification()
        {
            OnChanged?.Invoke(this);
        }

        public ModelApiEntityConfig GetEntity(int characterId)
        {
            return Configs.Find(x => x.id == characterId);
        }
        
        public ModelApiEntityConfig GetEntityUnMatched(int index)
        {
            return UnMatchedChars.Count > index ? UnMatchedChars[index] : null;
        }
        

        public async UniTask PrepareNextPool(int indexStartCheck, int maxElements = 3)
        {
            int count = Math.Min(maxElements, UnMatchedChars.Count);
            List<ModelApiEntityConfig> nextPool = new List<ModelApiEntityConfig>();

            for (int i = 0; i < count; i++)
            {
                int index = (indexStartCheck + i) % UnMatchedChars.Count;
                nextPool.Add(UnMatchedChars[index]);
            }

            for (int i = 0; i < nextPool.Count; i++)
            {
                var ele = nextPool[i];
                await AnR.LoadAddressable<Sprite>(ele.ClientCharId + "_bg");
                // var asset = AnR.Get<Sprite>(ele.ClientCharId + "_bg");
                // if (asset != null)
                // {
                //     UnityEngine.Debug.LogError("have asset: " + asset.name);
                // }
                // else
                // {
                //     UnityEngine.Debug.LogError("not found asset: " + ele.ClientCharId + "_bg");
                // }
            }
        }

        public ModelApiEntityExpDisplayData GetExpDisplay(ModelApiEntityConfig entityConfig)
        {
            return SpecialExtensionVer2.GetExpDisplay(entityConfig.level,entityConfig.exp,entityConfig.expRequire, ExpConfigs);
            // ModelApiEntityExpDisplayData data = new ModelApiEntityExpDisplayData();
            //
            // if (entityConfig.level > 0)
            // {
            //     int sumTotal = 0;
            //     for (int i = 0; i < entityConfig.level; i++)
            //     {
            //         sumTotal += ExpConfigs[i].EXP;
            //     }
            //
            //     data.start_exp = entityConfig.exp - sumTotal;
            //     data.end_exp = entityConfig.expRequire;
            //     data.total_exp_at_level = sumTotal + entityConfig.expRequire;
            // }
            // else
            // {
            //     //level 0
            //     data.start_exp = entityConfig.exp;
            //     data.end_exp = ExpConfigs[entityConfig.level].EXP;
            //     data.total_exp_at_level = ExpConfigs[entityConfig.level].EXP;
            // }
            //
            // return data;
        }
    }
}