// Author: ad   -
// Created: 02/12/2024  : : 01:12
// DateUpdate: 02/12/2024

using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Newtonsoft.Json;
using Template.Defines;
using Unity.VisualScripting;

namespace Game.Extensions
{
    public static class SpecialExtensionVer2
    {
        public static void GotoDatingWindow(this object source,ModelApiEntityConfig data,bool isFirstDating = false)
        {
            source.PostEvent(TypeGameEvent.OpenDating, new EventDataDating
            {
                entityConfig = data,
                isFirstDating = isFirstDating
            });
            Signal.Send(StreamId.UI.Dating);
        }
        
        public static void GotoWaifuProfileWindow(this object source,ModelApiEntityConfig data)
        {
            source.PostEvent(TypeGameEvent.OpenGallery, data);
            Signal.Send(StreamId.UI.OpenGallery);
        }

        public static void GotoEditProfileWindow(this object source,TypeFilterPanelCustomProfile type)
        {
            source.PostEvent(TypeGameEvent.EditProfile, type);
            Signal.Send(StreamId.UI.CustomProfile);
        }

        public static async void SaveProfile(this object source,TypeFilterPanelCustomProfile type,string value="")
        {
            if (!string.IsNullOrEmpty(value))
            {
                await FactoryApi.Get<ApiChatInfo>().PostEditProfile(type, value);
                ControllerPopup.ShowToastSuccess("Change profile success");
            }
            Signal.Send(StreamId.UI.OpenUserProfile);
        }

        public static void SaveProfileAvatar(string value)
        {
            var index = value.Split('_')[^1];
            var userInfo = FactoryStorage.Get<StorageUserInfo>();
            userInfo.Get().avatarSelected = int.Parse(index);
            userInfo.Save();
            ControllerPopup.ShowToastSuccess("Change profile success");
            Signal.Send(StreamId.UI.OpenUserProfile);
        }

        public static ModelApiEntityExpDisplayData GetExpDisplay(int level,int exp,int expRequire,List<ModelApiEntityExpConfig> expConfigs)
        {
            if (expRequire == -1)
                throw new Exception("Max Level");
            ModelApiEntityExpDisplayData data = new ModelApiEntityExpDisplayData();
            if (level > 0)
            {
                data.total_exp_at_level = expRequire;
                int modLv = expRequire - expConfigs[level - 1].EXP;
                data.running_exp_at_lv = exp - expConfigs[level - 1].EXP;
                data.end_exp_at_lv = modLv;
            }
            else
            {
                //level 0
                data.running_exp_at_lv = exp;
                data.end_exp_at_lv = expRequire;
                data.total_exp_at_level = expRequire;
            }

            UnityEngine.Debug.Log("Data exp: " + JsonConvert.SerializeObject(data));
            return data;
        }
        
        public static ModelApiEntityExpDisplayData GetExpDisplayOld(int level,int exp,int expRequire,List<ModelApiEntityExpConfig> expConfigs)
        {
            if (expRequire == -1)
                throw new Exception("Max Level");
            ModelApiEntityExpDisplayData data = new ModelApiEntityExpDisplayData();
     
            if (level > 0)
            {
                int sumTotal = 0;
                for (int i = 0; i < level; i++)
                {
                    UnityEngine.Debug.Log("i: " + i + " exp: " + expConfigs[i].EXP);
                    sumTotal += expConfigs[i].EXP;
                }

                data.running_exp_at_lv = exp - sumTotal;
                data.end_exp_at_lv = expRequire;
                data.total_exp_at_level = sumTotal + expRequire;
            }
            else
            {
                //level 0
                data.running_exp_at_lv = exp;
                data.end_exp_at_lv = expConfigs[level].EXP;
                data.total_exp_at_level = expConfigs[level].EXP;
            }

            return data;
        }
    }
}