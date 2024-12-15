// Author: 
// Created Date: 31/07/2024
// Update Time: 31/07

using System;
using Cysharp.Threading.Tasks;
using Game.Model;

namespace Game.Runtime
{
    public interface IServiceSyncData : IService
    {
        UniTask SyncForTapCount(int tapCount,Action callBack =null);
        void ForceSyncData(ModelApiGameInfo data);
    }
}