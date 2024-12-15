using System.Collections.Generic;
using Game.Model;

namespace Game.Runtime
{
    [Factory(StorageType.UserInfo, true)]
    public class StorageUserInfo : Storage<ModelStorageUserInfo>
    {
        public StorageUserInfo()
        {
            _key = GetKey(StorageType.UserInfo);
        }

        protected override void InitModel()
        {
            _model = new ModelStorageUserInfo()
            {
                enterTime = 0,
                exitTime = 0,
                sessionLength = 0,
                codeEnterGame = "",
                selectedWaifuId = 0,
                selectedBackgroundId = "",
                isChoosePremiumWaifu = false,
                isChooseSpecialBg = false,
                isDoneNotifySfwMode = false,
                isActiveSfwMode = false,
                selectedTapEffectId = 60000,
                // isCustomizedProfile = false,
                avatarSelected = 0,
                charSwipeIndexSelected = 0
            };
        }

        public override void Load()
        {
            base.Load();
            if (_model == null)
            {
                InitModel();
                Save();
            }
            else
            {
                if (_model.selectedWaifuId > 20009)
                {
                    _model.selectedWaifuId = 20009;
                    _model.isChoosePremiumWaifu = false;
                }
            }
        }
    }
}