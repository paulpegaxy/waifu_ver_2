using System;
using System.Collections.Generic;

namespace Game.Ton
{
    [Serializable]
    public class TonApiEvent
    {
        public bool is_incomplete;
        public TonApiEventTraceInfo trace_info;
    }

    [Serializable]
    public class TonApiEventTraceInfo
    {
        public string trace_state;

        public bool IsCompleted()
        {
            return trace_state == "complete";
        }
    }
}