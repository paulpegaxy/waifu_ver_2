using System;
using System.Collections.Generic;
using Game.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUserSubscriptionData
    {
        public string id;
        public DateTime timeEnd;
        public DateTime timeBuy;
    }
}