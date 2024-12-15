// Author:    -    ad
// Created: 08/07/2024  : : 12:19 AM
// DateUpdate: 08/07/2024

using System;
using Doozy.Runtime.Signals;
using Observer.EventData;
using UnityEngine.Events;

namespace Game.Extensions
{
    public static class SpecialExtensionObserver
    {
        public static void PostLobbyEvent(this object source, LobbyEventID id, object param = null)
        {
            Observer<LobbyEventID>.PostEvent(id, param);
        }

        public static void PostEvent<T>(this object source, T id, object param = null) where T : Enum
        {
            Observer<T>.PostEvent(id, param);
        }

        public static void RegisterEvent<T>(this object source, T id, Action<object> callback) where T : Enum
        {
            Observer<T>.RegisterListener(id, callback);
        }

        public static void RemoveEvent<T>(this object source, T id, Action<object> callback) where T : Enum
        {
            Observer<T>.RemoveListener(id, callback);
        }

        //Write a function to get Event Data of Observer class
        public static TData GetEventData<TEnum,TData>(this object source, TEnum id, bool isRemove = false) where TEnum : Enum
        {
            return Observer<TEnum>.GetData<TData>(id, isRemove);
        }

        public static void EmitEventWithSignal<T>(T id, object data = null) where T : Enum
        {
            Observer<T>.PostEvent(id, data);
            SignalsService.SendSignal(nameof(StreamId.Game), id.ToString());
        }
    }
}