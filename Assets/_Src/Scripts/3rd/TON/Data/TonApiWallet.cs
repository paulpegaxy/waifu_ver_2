using System;

namespace Game.Ton
{
    [Serializable]
    public class TonApiJettonWallet
    {
        public string address;
        public string balance;
        public string owner;
        public string jetton;
        public string last_transaction_lt;
        public string code_hash;
        public string data_hash;
    }
}