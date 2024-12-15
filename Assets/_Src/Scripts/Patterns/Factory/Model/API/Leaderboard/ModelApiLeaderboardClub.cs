using System;
using BreakInfinity;
using Newtonsoft.Json;

namespace Game.Model
{
    [Serializable]
    public class ModelApiLeaderboardClub
    {
        public int id;
        public string name;
        public string rank;
        public string total_point;
        public int total_members;
        public string telegram_link;
        public int index;
        

        [JsonIgnore] public BigDouble TotalPointParse => BigDouble.Parse(total_point);
    }
}