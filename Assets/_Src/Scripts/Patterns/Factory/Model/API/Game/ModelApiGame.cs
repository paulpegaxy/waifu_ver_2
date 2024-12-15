using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiGame
    {
        public ModelApiGameLogin Login;
        public ModelApiGameInfo Info;
        // public ModelApiGameInfoNew InfoNew;
        
        public ModelApiGameIdleEarning IdleEarning;

        public bool IsHaveProfitFromOffline()
        {
            return IdleEarning != null && (IdleEarning.free.PointParse > 0 || IdleEarning.premium.PointParse > 0);
        }
    }
}