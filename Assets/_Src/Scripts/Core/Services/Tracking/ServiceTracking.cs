// Author: ad   -
// Created: 04/10/2024  : : 00:10
// DateUpdate: 04/10/2024

using System;

namespace Game.Runtime
{
    public class ServiceTracking : IServiceTracking
    {
        public async void SendTrackingSessionLength()
        {
            var storageUser = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUser.Get();
            if (userInfo.enterTime > 0 && userInfo.exitTime > userInfo.enterTime)
            {
                await FactoryApi.Get<ApiTracking>().PostTracking();
                return;
            }

            userInfo.enterTime = DateTime.UtcNow.ToUnixTimeSeconds();
            // UnityEngine.Debug.LogError("Enter time: " + userInfo.enterTime);
            userInfo.exitTime = userInfo.enterTime;
            storageUser.Save();
        }

        public void UpdateExitTime()
        {
            var storageUser = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUser.Get();
            userInfo.exitTime = DateTime.UtcNow.ToUnixTimeSeconds();
            
            storageUser.Save();
        }
    }
}