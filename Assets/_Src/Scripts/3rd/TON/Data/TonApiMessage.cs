using System;
using System.Collections.Generic;

namespace Game.Ton
{
    [Serializable]
    public class TonApiMessages
    {
        public List<TONMessageData> messages;
    }

    [Serializable]
    public class TONMessageData
    {
        public string hash;
        public TonMessageContent message_content;
    }

    [Serializable]
    public class TonMessageContent
    {
        public TonMessageContentDecoded decoded;
    }

    [Serializable]
    public class TonMessageContentDecoded
    {
        public string type;
        public string comment;
    }
}