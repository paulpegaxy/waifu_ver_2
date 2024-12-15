using System;
using BreakInfinity;

namespace Game.Model
{
    [Serializable]
    public class ModelLeaderboardAllTime
    {
        public string Name;
        public int Rank;
        public BigDouble LifeTimeScore;
    }
}