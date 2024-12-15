using System;
using Sirenix.OdinInspector;

namespace Game.Model
{
    public enum TypeGameConfig
    {
        TileConfig,
        GamePlayConfig,
        RandomSkill
    }

    [Serializable]
    public class ModelGameConfig
    {
        public TypeGameConfig Type;
        [ShowInInspector]
        public ModelGameConfigParam Param;
    }
}