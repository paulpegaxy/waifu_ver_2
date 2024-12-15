// Author: ad   -
// Created: 15/12/2024  : : 14:12
// DateUpdate: 15/12/2024

using System;
using System.Collections.Generic;
using Game.Extensions;

namespace Game.Model
{
    [Serializable]
    public class ModelApiGameConfig : ModelApiNotification<ModelApiGameConfig>
    {
        public List<ModelApiEntityExpConfig> userLevel;
        public List<ModelApiEntityExpConfig> attractionLevel;
        public List<ModelApiSubscriptionConfig> subscription;
        
        public override void Notification()
        {
            OnChanged?.Invoke(this);
        }
    }
}