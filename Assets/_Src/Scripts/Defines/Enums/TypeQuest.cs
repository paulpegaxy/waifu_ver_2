using System;

namespace Template.Defines
{
    [Flags]
    public enum QuestCategory
    {
        Daily,
        Basic,
        X,
        Telegram,
        Checkin,
        Purchase,
        Invite,
        InvitePremium,
        Partner,
        NotcoinCollab,
        Event,
        None,
    }

    public enum QuestType
    {
        Common,
        DailyCheckIn,
        DailyLogin,
        DailyPurchase,
        DailyInvite,
        JoinAnnouncement,
        ShareStory,
        WatchAds,
        BoostChannel,
        PartnerCheckIn,
        MeetYuki,
        UnlockWaifu,
        InviteFriend,
        Purchase,
        TonCheckInVerify,
        NameIcon,
        CheckInJackpot
    }
}