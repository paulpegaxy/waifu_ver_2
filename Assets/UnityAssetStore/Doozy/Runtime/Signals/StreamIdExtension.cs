// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using UnityEngine;
// ReSharper disable All

namespace Doozy.Runtime.Signals
{
    public partial class Signal
    {
        public static bool Send(StreamId.Game id, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), message);
        public static bool Send(StreamId.Game id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalSource, message);
        public static bool Send(StreamId.Game id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.Game id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.Game id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.Game id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.Game id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.Game id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Game), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.UI id, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), message);
        public static bool Send(StreamId.UI id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalSource, message);
        public static bool Send(StreamId.UI id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.UI id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.UI id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.UI id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.UI id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.UI id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.UI), id.ToString(), signalValue, signalSender, message);   
    }

    public partial class StreamId
    {
        public enum Game
        {
            Init,
            Load,
            Loaded
        }

        public enum UI
        {
            BackToMain,
            BackToSwipe,
            ChatList,
            Club,
            ClubBoost,
            ClubDetail,
            ClubRandom,
            CustomProfile,
            Dating,
            GalleryDetail,
            Jackpot,
            OpenEventBundle,
            OpenEventYuki,
            OpenFriend,
            OpenGallery,
            OpenIdleEarn,
            OpenPartner,
            OpenPlayerSummary,
            OpenQuest,
            OpenShop,
            OpenUpgrade,
            OpenUserProfile,
        }         
    }
}

