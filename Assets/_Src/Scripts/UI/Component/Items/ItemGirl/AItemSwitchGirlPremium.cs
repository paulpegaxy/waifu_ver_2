// Author: ad   -
// Created: 17/10/2024  : : 03:10
// DateUpdate: 17/10/2024

using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public abstract class AItemSwitchGirlPremium : AItemCheckStartGame
    {
        protected int InitGirlId;
        protected bool IsPremium;
        
        protected override void OnInit()
        {
            // UnityEngine.Debug.LogError("item : " + gameObject.name);
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            // if (userInfo.selectedWaifuId == 0)
            // {
            //     userInfo.selectedWaifuId = FactoryApi.Get<ApiGame>().Data.Info.CurrentGirlId;
            //     userInfo.isChoosePremiumWaifu = false;
            //     storageUserInfo.Save();
            // }
            
            // UnityEngine.Debug.LogError("id: "+userInfo.selectedWaifuId+" , "+userInfo.isChoosePremiumWaifu);
            IsPremium = userInfo.isChoosePremiumWaifu;
            InitGirlId = userInfo.selectedWaifuId;
            SwitchGirl(IsPremium);
        }

        protected override void OnEnabled()
        {
            var data = this.GetEventData<TypeGameEvent, int>(TypeGameEvent.ChangeGirl);
            if (data > 0)
            {
                OnChangeGirl(data);
            }
        }
        
        private void OnChangeGirl(int girlId)
        {
            // if (girlId != InitGirlId)
            // {
                var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
                IsPremium = userInfo.isChoosePremiumWaifu;
                InitGirlId = girlId;
                SwitchGirl(IsPremium);
            // }
        }

        
        protected abstract void SwitchGirl(bool isPremium);
    }
}