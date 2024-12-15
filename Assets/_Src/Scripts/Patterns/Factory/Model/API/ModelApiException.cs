using System;

namespace Game.Model
{
    [Serializable]
    public class ModelApiException
    {
        public int code;
        public string message;
        public string description;
        public ModelApiExceptionError error;
    }

    public class ModelApiExceptionError
    {
        public int statusCode;
        public string message;
        public string stack;
    }
}