using System.Collections;
using System.Collections.Generic;
using Game.Model;
using UnityEngine;

namespace Game.Runtime
{
    [Factory(CollectionType.Rarities, true)]
    public class CollectionRarities : Collection<List<ModelRarity>>
    {
        public CollectionRarities()
        {
            _key = GetKey(CollectionType.Rarities);
        }

        public ModelRarity Get(int rarityId)
        {
            return _model.Find(x => x.Param.ID == rarityId);
        }
    }

}
