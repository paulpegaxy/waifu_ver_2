using System;
using System.Collections.Generic;
using BreakInfinity;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEventRanking
    {
        public List<ModelApiEventRankingData> list;
        public ModelApiEventRankingData user;
    }

    [Serializable]
    public class ModelApiEventRankingData
    {
        public int rank_top;
        public int user_id;
        public string rank;
        public string name;
        public string gold = "0";

        public BigDouble Gold => BigDouble.Parse(gold);
    }
}