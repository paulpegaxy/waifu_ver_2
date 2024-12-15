using System;

namespace Game.Model
{
    [Serializable]
    public class ModelApiChatAIReply
    {
        public ModelApiChatHistory reply;
        public int chat_point;
    }
}