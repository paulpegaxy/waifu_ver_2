// Author: 
// Created Date: 31/07/2024
// Update Time: 31/07

using System;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
    public class ServiceSyncData : IServiceSyncData
    {
        public async UniTask SyncForTapCount(int tapCount, Action callBack = null)
        {
            int tempTapCount = tapCount;
            // GameUtils.Log("green","Send tap count: " + tempTapCount);
            if (ControllerAutomation.IsStarted)
            {
                GameUtils.Log("red",
                    "from bot tap: " + ControllerAutomation.AutoTapCount + ",  point perTAp: " +
                    FactoryApi.Get<ApiGame>().Data.Info.PointPerTapParse);
                tempTapCount += ControllerAutomation.AutoTapCount;
                ControllerAutomation.AutoTapCount = 0;
                // GameUtils.Log("red","Final Tap: " + tempTapCount);
            }

            if (tempTapCount > 0)
            {
                var data=await FactoryApi.Get<ApiGame>().PostTapCount(tempTapCount);
                var amountPoint = ControllerResource.Get(TypeResource.HeartPoint).Amount;
                var amountStamina = ControllerResource.Get(TypeResource.ExpWaifu).Amount;
                // if (amountPoint < data.PointParse)
                // {
                    ControllerResource.Set(TypeResource.HeartPoint, data.PointParse);
                // }

                // if (amountStamina > data.StaminaParse)
                // {
                    ControllerResource.Set(TypeResource.ExpWaifu,data.StaminaParse);
                // }
            }
            // else
            // {
            //     await ApiGame.GetInfo();
            // }
            
            callBack?.Invoke();
        }

        public void ForceSyncData(ModelApiGameInfo data)
        {
            ControllerResource.Set(TypeResource.HeartPoint, data.PointParse);
            ControllerResource.Set(TypeResource.ExpWaifu, data.StaminaParse);
            ControllerResource.Set(TypeResource.Berry, data.BerryParse);
        }
    }
}