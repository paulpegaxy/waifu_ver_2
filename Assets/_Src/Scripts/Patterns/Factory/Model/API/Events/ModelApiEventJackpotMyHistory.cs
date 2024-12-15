using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEventJackpotMyHistory
    {
        public string date;
        public bool is_win;
        public List<int> tickets;
    }
}