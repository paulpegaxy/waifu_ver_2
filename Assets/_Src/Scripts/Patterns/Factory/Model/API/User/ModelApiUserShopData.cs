using System;
using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUserShopData
    {
        public bool is_auto_bot;
        public bool is_first_purchase;
        public List<string> received_bonus = new();
        public TypeMessageGirl type_message = TypeMessageGirl.Regular;
    }
}