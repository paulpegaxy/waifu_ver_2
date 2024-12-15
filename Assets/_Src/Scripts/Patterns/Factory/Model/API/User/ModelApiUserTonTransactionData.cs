using System;
using Game.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUserTonTransactionData : ModelApiClaim
    {
        public int user_id;
        public string source;
        public string msg_hash;
        public DateTime created_at;
    }
}