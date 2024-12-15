using System;
using BreakInfinity;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEventLeaderboard
    {
        public ModelApiUserData user;
        public int rank;
        public string value;

        public BigDouble ValueParse => BigDouble.Parse(value);
    }
}