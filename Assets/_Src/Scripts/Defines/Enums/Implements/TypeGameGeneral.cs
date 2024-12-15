// Author:    -    ad
// Created: 18/07/2024  : : 11:11 PM
// DateUpdate: 18/07/2024

namespace Template.Defines
{
    public enum TypeGameGeneral
    {

    }

    public enum GameAction
    {
        PetBuy,
        PetMerge,
        PetSell,
        FirstTapGirl,
        TapToUndressGirl,
        FirstUndressGirl,
    }

    public enum AudioMixerType
    {
        Master = -1,
        Bgm,
        Sfx,
    }

    public enum TypeQuality
    {
        Low,
        High,
    }

    public enum TypePurchase
    {
        Free,
        Ton,
        Pal,
    }

    public enum RefreshTimerType
    {
        RefreshToken,
        GameInfo,
        ServerTime,
    }
}