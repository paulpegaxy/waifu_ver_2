using System;
using Game.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUserTonHistoryData
    {
        public int id;
        public float amount;
        public string type;
        public string status;
        public string source;
        public string tx_hash;
        public string to_address;
        public long? deadline;
        public string signature;
        public string order_id;
        public ModelApiUserTonHistoryExtraData extra_data;
        public DateTime created_at;
    }

    [Serializable]
    public class ModelApiUserTonHistoryExtraData
    {
        public string description;
        public string contract_address;
    }
}