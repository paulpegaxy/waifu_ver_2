namespace Template.Defines
{
    public enum TutorialCategory
    {
        None,
        Main,
        Booster,
        Upgrade,
        Undress,
        GameFeature,
        NextGirl,
    }

    public enum TutorialState
    {
        MainFirstTimeLogin,
        MainFirstWaitTapGirl,
        MainPointCurrency,
        MainFirstDone,
        
        MainBooster,
        MainBoosterConfirmUpgrade,
        MainBoosterActionUpgrade,
        MainBoosterBack,
        MainBoosterGuideTap,
        MainBoosterDone,
        
        Upgrade,
        UpgradeConfirm,
        UpgradeActionFirstSkill,
        UpgradeBack,
        UpgradeGuideProfit,
        UpgradeDone,
        
        Undress,
        UndressDone,
        
        FeatureRanking,
        FeatureRankingWife,
        FeatureRankingBack,
        FeatureTapBot,
        FeatureQuest,
        FeatureQuestBack,
        FeatureShop,
        FeatureShopTimelapse,
        FeatureDone,
        
        NextGirl,
        NextGirlConfirm,
        NextGirlGallery,
        NextGirlDone,
        
        
        MainLeaderboard,
        MainLeaderboardScore,
        MainLeaderboardBack,
        MainFriend,
        MainFriendReferral,
        MainFriendClickBuddyTree,
        MainFriendBuddyTree,
        MainFriendBack,
        MainQuest,
        MainGoodbye,
        MainCompleted
    }

    public enum TutorialAlignment
    {
        Top,
        TopCenter1,
        TopCenter2,
        Center,
        CenterBottom1,
        CenterBottom2,
        Bottom,
    }

    public enum TutorialObject
    {
        None,
        MainPointCurrency,
        MainLeaderboard,
        MainLeaderboardScore,
        MainLeaderboardBack,
        MainFriend,
        FriendContentBonus,
        FriendBuddyTreeTab,
        FriendBuddyTreeHeader,
        FriendBack,
        ButtonMergeConfirm,
        MainClub,
        MainPet,
        MainQuest,
        MainShop,
        MainUpgrade,
        MainUpgradeBack,
        MainTapGirl,
        MainBooster,
        BoosterConfirm,
        BoosterActionUpgrade,
        BoosterBack,
        BoosterGuideTap,
        UpgradeConfirm,
        UpgradeActionConfirm,
        UpgradeBack,
        MainProfitPerHour,
        MainUndress,
        MainNextGirl,
        MainTapBot,
        ShopTimelapse,
        RankingWife,
        RankingBack,
        QuestBack,
        ShopBack,
        NextgirlHightlight,
        MainGallery,
        MainTapToShowUndress
    }
}