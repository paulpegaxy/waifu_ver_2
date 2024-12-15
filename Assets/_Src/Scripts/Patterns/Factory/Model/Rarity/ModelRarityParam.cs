using System.Collections;
using System.Collections.Generic;
using Game.Runtime;
using UnityEngine;


namespace Game.Model
{
    public class ModelRarityParam
    {
        public int ID;
        public string Name;
        public int MaxLevelUnlock;
    }

    public class FactoryModelRarityParam : Factory<TypeRarity, ModelRarityParam>
    {
        
    }
}