using System;
using Game.Runtime;

namespace Game.Model
{
    [Serializable]
    [Factory(TypeGameConfig.GamePlayConfig)]
    public class ModelGameConfigParamGameplay : ModelGameConfigParam
    {
        public int startTurn = 10;
        public int initGateHp = 1000;
        public int initExp = 300;
    }
}