using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiCommonServerInfo
    {
        public long utc_time;
        public bool is_maintenance;
        public List<string> wl;

        public bool IsMatchWl(string teleId)
        {
            return wl.Contains(teleId);
        }
    }
}