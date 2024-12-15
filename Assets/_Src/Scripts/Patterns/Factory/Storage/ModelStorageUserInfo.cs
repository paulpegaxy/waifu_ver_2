

using ExcelDataReader.Log;
using UnityEngine.Serialization;

namespace Game.Model
{
    [System.Serializable]
    public class ModelStorageUserInfo
    {
        public long enterTime;
        public long exitTime;
        public long sessionLength;
        public string codeEnterGame;
        
        public int selectedWaifuId;
        public string selectedBackgroundId;
        
        public bool isChoosePremiumWaifu;
        public bool isChooseSpecialBg;

        public int selectedTapEffectId;
        
        public bool isDoneNotifySfwMode;
        public bool isActiveSfwMode;

        // public bool isCustomizedProfile;
        public int avatarSelected;
        public int charSwipeIndexSelected;
    }
}