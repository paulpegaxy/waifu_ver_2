using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiClaim
    {
        public int id;
        public string order_id;
        public string currency;
        public double amount;
        public string signature;
        public long? deadline;
        public string status;
        public string to_address;
        public string contract_address;
    }

    [Serializable]
    public class ModelApiClaimComplete
    {

    }

    [Serializable]
    public class ModelApiClaimHistory
    {
        public int total;
        public int offset;
        public int limit;
        public List<ModelApiClaim> data;
    }
}