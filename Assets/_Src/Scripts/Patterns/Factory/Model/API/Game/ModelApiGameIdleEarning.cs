using System;
using BreakInfinity;

namespace Game.Model
{
    [Serializable]
    public class ModelApiGameIdleEarning
    {
        public ModelApiGameIdlEarningFree free;
        public ModelApiGameIdlEarningPremium premium;
    }

    [Serializable]
    public class ModelApiGameIdlEarningFree
    {
        public string point;
        public float time;
        
        public BigDouble PointParse => BigDouble.Parse(point);
    }

    [Serializable]
    public class ModelApiGameIdlEarningPremium
    {
        public string point;
        public int price;
        public string currency;
        public float time;
        
        public BigDouble PointParse => BigDouble.Parse(point);
    }
}