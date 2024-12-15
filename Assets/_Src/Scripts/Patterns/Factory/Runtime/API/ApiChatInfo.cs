using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.UI;
using Newtonsoft.Json;
using Template.Defines;

namespace Game.Runtime
{
    [Factory(ApiType.ChatInfo, true)]
    public class ApiChatInfo : Api<ModelApiChatInfo>
    {
        public async UniTask<ModelApiChatInfoDetail> GetInfo()
        {
            var data = await Get<ModelApiChatInfoDetail>("/v1/chat/info", "data");
            SyncData(data);
            return data;
        }

        public async UniTask<ModelApiGameConfig> GetConfig()
        {
            var data = await Get<ModelApiGameConfig>("/v1/chat/config", "data");
            Data.GameConfig = data;
            GameUtils.Log("blue", "subscription configs:     " + JsonConvert.SerializeObject(Data.GameConfig.subscription));
            GameUtils.Log("blue", "user lv configs:     " + JsonConvert.SerializeObject(Data.GameConfig.userLevel));
            Data.GameConfig.Notification();
            return data;
        }

        private void SyncData(ModelApiChatInfoDetail info)
        {
            Data.Info = info;
            Data.Info.Notification();
            ControllerResource.Set(TypeResource.ChatPoint, info.chat_point);
        }
        
        // public async void PostMatchGirl()
        // {
        //     Data.Info.swipe_count--;
        //     Data.Info.Notification();
        // }

        // public async UniTask PostSendChatGirl()
        // {
        //     // Data.Info.chat_point -= Data.Info.price_send_chat;
        //     SyncData(Data.Info);
        // }

        public async UniTask<bool> PostCustomProfile(Dictionary<string, string> dictData)
        {
            var bodyPostDict = new Dictionary<string, string>();
            var keysToCheck = new[] { "name", "interested_in", "zodiac","genres", "ava_index" };

            foreach (var key in keysToCheck)
            {
                if (dictData.TryGetValue(key, out var value))
                {
                    bodyPostDict.Add(key, value);
                }
            }

            // UnityEngine.Debug.Log("PostCustomProfile: " + JsonConvert.SerializeObject(bodyPostDict));
            
            var status = await Post<bool>("/v1/chat/setProfile", "status", bodyPostDict);
            await GetInfo();
            return status;
        }

        public async UniTask PostEditProfile(TypeFilterPanelCustomProfile type, string value)
        {
            var bodyPostDict = new Dictionary<string, string>();
            var keysToCheck = new[]
            {
                TypeFilterPanelCustomProfile.name,
                TypeFilterPanelCustomProfile.interested_in,
                TypeFilterPanelCustomProfile.zodiac,
                TypeFilterPanelCustomProfile.genres
            };
            

            foreach (var key in keysToCheck)
            {
                if (key == type)
                {
                    bodyPostDict.Add(key.ToString(), value);
                }
                else
                {
                    string valueSend = "";
                    var extraInfo = Data.Info.extra_data;
                    switch (key)
                    {
                        case TypeFilterPanelCustomProfile.name:
                            valueSend = extraInfo.name;
                            break;
                        case TypeFilterPanelCustomProfile.interested_in:
                            valueSend = extraInfo.interested_in;
                            break;
                        case TypeFilterPanelCustomProfile.zodiac:
                            valueSend = extraInfo.zodiac;
                            break;
                        case TypeFilterPanelCustomProfile.genres:
                            valueSend = extraInfo.genres;
                            break;
                    }

                    bodyPostDict.Add(key.ToString(), valueSend);
                }
            }
            
            await Post<bool>("/v1/chat/setProfile", "status", bodyPostDict);
            await GetInfo();
        }


        public async UniTask PostCheat(int chat_point=100,int swipe_count=10)
        {
            var status = await Post<bool>("/v1/chat/cheat-currency", "status", new {chat_point, swipe_count});
            await GetInfo();
        }
    }
}