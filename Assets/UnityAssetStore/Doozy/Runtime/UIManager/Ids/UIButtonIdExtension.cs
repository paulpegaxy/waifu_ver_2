// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Components
{
    public partial class UIButton
    {
        public static IEnumerable<UIButton> GetButtons(UIButtonId.Club id) => GetButtons(nameof(UIButtonId.Club), id.ToString());
        public static bool SelectButton(UIButtonId.Club id) => SelectButton(nameof(UIButtonId.Club), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Friend id) => GetButtons(nameof(UIButtonId.Friend), id.ToString());
        public static bool SelectButton(UIButtonId.Friend id) => SelectButton(nameof(UIButtonId.Friend), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Main id) => GetButtons(nameof(UIButtonId.Main), id.ToString());
        public static bool SelectButton(UIButtonId.Main id) => SelectButton(nameof(UIButtonId.Main), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.MainV2 id) => GetButtons(nameof(UIButtonId.MainV2), id.ToString());
        public static bool SelectButton(UIButtonId.MainV2 id) => SelectButton(nameof(UIButtonId.MainV2), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Navigation id) => GetButtons(nameof(UIButtonId.Navigation), id.ToString());
        public static bool SelectButton(UIButtonId.Navigation id) => SelectButton(nameof(UIButtonId.Navigation), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Popup id) => GetButtons(nameof(UIButtonId.Popup), id.ToString());
        public static bool SelectButton(UIButtonId.Popup id) => SelectButton(nameof(UIButtonId.Popup), id.ToString());
        public static IEnumerable<UIButton> GetButtons(UIButtonId.Shop id) => GetButtons(nameof(UIButtonId.Shop), id.ToString());
        public static bool SelectButton(UIButtonId.Shop id) => SelectButton(nameof(UIButtonId.Shop), id.ToString());
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIButtonId
    {
        public enum Club
        {
            Player
        }

        public enum Friend
        {
            BuddyInformation,
            Detail,
            Event,
            Leaderboard
        }

        public enum Main
        {
            Airdrop,
            Booster,
            ClubDetail,
            ClubRandom,
            EventYuki,
            Friend,
            Gallery,
            Generate,
            GuideProfit,
            IdleEarn,
            LifetimeRanking,
            Mail,
            NextGirl,
            PalsStore,
            PartnerMergePal,
            Quest,
            Ranking,
            Setting,
            Shop,
            TapBot,
            UndressGirl,
            UserProfile
        }

        public enum MainV2
        {
            Swipe
        }

        public enum Navigation
        {
            Back
        }

        public enum Popup
        {
            BoosterConfirm,
            IdleEarnConfirm,
            MergeConfirm
        }
        public enum Shop
        {
            Timelapse
        }    
    }
}
