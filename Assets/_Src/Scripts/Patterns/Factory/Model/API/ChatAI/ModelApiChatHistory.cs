using System;

namespace Game.Model
{
    [Serializable]
    public class ModelApiChatHistory
    {
        public int id;
        public int sender;
        public string message;
        public TypeSenderChatHistory sender_type;
        public ModelApiChatHistoryExtra extra_data;

        public string ShortMessage()
        {
            return message.Truncate(GameConsts.MAX_COUNT_CHAR_CHAT);
        }

        public string GetPictureMessage()
        {
            if (extra_data != null && !string.IsNullOrEmpty(extra_data.image))
                return extra_data.image;
            
            return string.Empty;
        }
    }

    public enum TypeSenderChatHistory
    {
        user,
        character
    }

    [Serializable]
    public class ModelApiChatHistoryExtra
    {
        public string image;
    }
}