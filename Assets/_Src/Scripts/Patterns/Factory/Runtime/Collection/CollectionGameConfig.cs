using System.Collections.Generic;
using Game.Model;
using UnityEngine;

namespace Game.Runtime
{
    [Factory(CollectionType.GameConfig, true)]
    public class CollectionGameConfig : Collection<List<ModelGameConfig>>
    {
        public CollectionGameConfig()
        {
            _key = GetKey(CollectionType.GameConfig);
        }

        public T Get<T>(TypeGameConfig type) where T : ModelGameConfigParam
        {
            return (T) _model.Find(x => x.Type == type).Param;
        }

        public override void Init()
        {
            base.Init();
        }
    }
}