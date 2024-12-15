using System;
using System.Collections.Generic;
using Game.Extensions;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiChatInfo
    {
        public ModelApiChatInfoDetail Info;
        public ModelApiGameConfig GameConfig;

        public bool IsHaveProfile => Info.extra_data != null && !string.IsNullOrEmpty(Info.extra_data.interested_in);

        public ModelApiEntityExpDisplayData GetExpDisplay()
        {
            return SpecialExtensionVer2.GetExpDisplay(Info.level, Info.exp, Info.expRequire, GameConfig.userLevel);
        }
    }
}