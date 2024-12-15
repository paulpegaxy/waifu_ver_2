using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Runtime
{
    
    [Factory(ApiType.Tracking, true)]
    public class ApiTracking : Api<ModelApiTracking>
    {
        
        public async UniTask PostTracking()
        {
#if !UNITY_EDITOR
            var storageUser = FactoryStorage.Get<StorageUserInfo>();
            var userInfo=storageUser.Get();
            // UnityEngine.Debug.Log("Time enter: " + userInfo.enterTime + ", Time exit: " + userInfo.exitTime);
            if (userInfo.enterTime <= 0)
            {
                userInfo.enterTime = DateTime.UtcNow.ToUnixTimeSeconds();
                userInfo.exitTime =  DateTime.UtcNow.ToUnixTimeSeconds();
                storageUser.Save();
                return;
            }
            
            userInfo.sessionLength = (long)(userInfo.exitTime - userInfo.enterTime);
            // UnityEngine.Debug.Log("Time exit: " + userInfo.exitTime+", session length: "+(userInfo.exitTime - userInfo.enterTime));
            storageUser.Save();
            
            Dictionary<string, string> dictData = new();
            dictData.Add("enterTime", userInfo.enterTime.ToString());
            dictData.Add("exitTime", userInfo.exitTime.ToString());
            dictData.Add("sessionLength", userInfo.sessionLength.ToString());
            
            string platform = "";

             if (TelegramWebApp.IsMobile())
             {
                 platform = "Mobile";
                 var platformDetect = TelegramWebApp.Platform();
                 if (!string.IsNullOrEmpty(platformDetect))
                     platform = platformDetect;
             }
            else
            {
                platform = "Desktop";
            }
            
            if (!string.IsNullOrEmpty(platform))
                dictData.Add("platform", platform);
            
            var dataPost = JsonConvert.SerializeObject(dictData);
            // UnityEngine.Debug.LogError("dataPost: " + dataPost);

            await Post<string>("/v1/game/tracking", "data", new { data = dataPost });
            userInfo.enterTime = DateTime.UtcNow.ToUnixTimeSeconds();
            userInfo.exitTime = userInfo.enterTime;
            userInfo.sessionLength = 0;
            storageUser.Save();
#endif
        }
    }
}