using System;
using System.Collections.Generic;

namespace Game.Ton
{
    [Serializable]
    public class TonApiTransactions
    {
        public List<TONTransactionData> transactions;
    }

    [Serializable]
    public class TONTransactionData
    {
        [Serializable]
        public class Message
        {
            public string hash;
            public bool? bounced;
        }

        public List<Message> out_msgs;
    }
}