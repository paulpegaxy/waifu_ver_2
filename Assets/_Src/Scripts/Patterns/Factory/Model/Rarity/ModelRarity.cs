using System;
using Sirenix.OdinInspector;

namespace Game.Model
{
    public enum TypeRarity
    {
        COMMON,
        RARE,
        EPIC,
        MYTHICAL,
        LEGENDARY,
    }

    [Serializable]
    public class ModelRarity
    {
        public TypeRarity Type;
        [ShowInInspector] public ModelRarityParam Param = new();
    }
}