// Author: 
// Created Date: 19/07/2024
// Update Time: 19/07

namespace Template.Defines
{
    public enum TypeGameEvent
    {
        Load,
        GameStart,
        Club,
        ClubDetail,
        ShopOpenFromIdleEarning,
        ClubBoost,
        GalleryDetail,
        UndressGirl,
        UndressTransitionFade,
        BotSendAutoTap,
        OpenMessage,
        NextGirlSuccess,
        ClaimAchievementSuccess,
        TapForTutorial,
        FristTapGirlTut,
        ReloadFeature,
        InterruptGame,
        ChangeGirl,
        OpenPremiumGallery,
        SuccessBuyOffer,
        ChangeSfwMode,
        ActiveTutorialUndress,
        ChangeBackground,
        Partner,
        OpenEventBundle,
        ChangeTapEffect,
        OpenDating,
        OpenGallery,
        NeedRefreshChatHistory,
        HideInputField,
        OnEndEditInputFieldWebGl,
        EditProfile,
    }


    public enum GameEventLoadType
    {
        LoadToGame,
        LoadToMain,
    }
}