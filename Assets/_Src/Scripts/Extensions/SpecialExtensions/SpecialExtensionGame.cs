
using System;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Game.UI;
using JetBrains.Annotations;
using Template.Defines;
using UnityEngine;

public static class SpecialExtensionGame
{
    public static int MyUserId => FactoryApi.Get<ApiUser>().Data.User.Id;

    public static string MyTelegramId => FactoryApi.Get<ApiUser>().Data.User.telegram_id;

    public static void Invite()
    {
        var gameUrl = GetInviteReferralLink();
        var text = Localization.Get(TextId.Friend_InviteText);

        // var url = $"https://telegram.me/share/url?url={gameUrl}&text={System.Uri.EscapeDataString(text)}";

        var url = $"https://t.me/share/url?url={gameUrl}&text={System.Uri.EscapeDataString(text)}";

        TelegramWebApp.OpenLink(url);
    }

    public static string GetInviteReferralLink()
    {
        var apiUser = FactoryApi.Get<ApiUser>();
        var referralCode = apiUser.Data.User.referral_code;

#if PRODUCTION_BUILD && !TEST_PRODUCTION_BUILD
			return $"https://t.me/pocketwaifu_bot/game?startapp={referralCode}";
#elif TEST_PRODUCTION_BUILD
         return $"https://t.me/pocketwaifu_bot/gamethieunhi2k?startapp={referralCode}";
#else
        return $"https://t.me/wai124578_bot/wai12dev?startapp={referralCode}";
#endif
    }

    public static string GetBotUsername()
    {
#if PRODUCTION_BUILD
			return $"pocketwaifu_bot";
#else
        return $"wai124578_bot";
#endif
    }

    public static bool ShareToStoryForRanking(int rank)
    {
        var botname = "wai124578_bot";
#if PRODUCTION_BUILD
        botname = "pocketwaifu_bot";
#elif TEST_PRODUCTION_BUILD
        botname = "wai124578_bot";
#endif
        var inviteLink = GetInviteReferralLink();
        inviteLink += "_story";

        // var storyConfig = FactoryApi.Get<ApiQuest>().Data.Configs.GetStoryVideoLinkForRanking();
        // if (storyConfig == null)
        // {
        //     return false;
        // }

        var text = string.Format(Localization.Get(TextId.Toast_ShareStoryRanking), rank, inviteLink, botname);
        var mediaUrl =
            $"https://cdn.mirailabs.co/waifu-tap/static/pocket_waifu_story_updated.mp4?{ServiceTime.CurrentUnixTime}";

        // _ = FactoryApi.Get<ApiQuest>().StoryCheck(storyConfig.quest_id);

        return TelegramWebApp.ShareToStory(mediaUrl, text, inviteLink, "Play");
    }

    public static bool ShareToStory(string text, string mediaUrl)
    {
        var botname = "wai124578_bot";
#if PRODUCTION_BUILD
    			botname = "pocketwaifu_bot";
#endif
        var inviteLink = GetInviteReferralLink();
        text = string.Format(text, inviteLink, botname).Replace("\\n", "\n");
        mediaUrl += $"?t={ServiceTime.CurrentUnixTime}";

        return TelegramWebApp.ShareToStory(mediaUrl, text, inviteLink, "Play");
    }


    public static bool IsAutoBotPurchased()
    {
        var apiUser = FactoryApi.Get<ApiUser>().Data.Game;
        if (apiUser == null) return false;

        return apiUser.auto_bot.is_auto_bot;
    }

    public static bool IsAutoBotTrial()
    {
        var apiUser = FactoryApi.Get<ApiUser>();
        if (apiUser.Data.Game == null) return false;

        var autoBotField = apiUser.Data.Game.auto_bot;

        return autoBotField.IsUseBotTrial;
    }

    public static bool IsFeatureUnlocked()
    {
        var apiGame = FactoryApi.Get<ApiGame>();
        return apiGame.Data.Info != null && apiGame.Data.Info.highest_level > 2;
    }

    public static string NotiNotEnoughPoint()
    {
        return Localization.Get(TextId.Toast_NotEnoughSc /**/);
    }

    public static bool CanRestoreStamina()
    {
        var apiGame = FactoryApi.Get<ApiGame>().Data.Info;

        var apiUpgrade = FactoryApi.Get<ApiUpgrade>().Data;

        // Debug.Log("Next Charge Stamina Wait Time: " + apiGame.next.next_charge_stamina_wait_time + ", remain count: " + apiUpgrade.current.charge_stamina);

        if (apiUpgrade.current.charge_stamina <= 0)
        {
            return false;
        }



        if (apiGame.next.next_charge_stamina_wait_time > GameConsts.BOT_TRIAL_HARD_CHECK_TIME)
            return false;

        return true;
    }


    public static async void PlayBotTrial(Action callback = null, UIPopup popupConfirm = null)
    {
        ControllerPopup.SetApiLoading(true);
        try
        {
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            storageSetting.Get().isUseBotTap = true;
            storageSetting.Save();
            await FactoryApi.Get<ApiUser>().PostGetTrialBot();
            if (popupConfirm != null)
                popupConfirm.Hide();
            callback?.Invoke();
            ControllerAutomation.Start(false);
            ControllerPopup.SetApiLoading(false);
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }

    public static Color GetColorTextPrice(TypeResource typeCurrency, BigDouble priceRequire,
        TypeColor colorDefault = TypeColor.WHITE)
    {
        var currPoint = ControllerResource.Get(typeCurrency).Amount;
        var visualConfig = DBM.Config.visualConfig;
        return currPoint < priceRequire
            ? visualConfig.GetColorStatus(TypeColor.RED)
            : visualConfig.GetColorStatus(colorDefault);
    }

    public static void SetDataCardConfirmIdleEarn(ref ConfirmIdleEarnManageCard card, string id, int level,
        BigDouble pointPerHour)
    {
        card.LoadData(id, level, pointPerHour);
    }

    public static void SaveNextGirl(int girlID)
    {
        var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
        var userInfo = storageUserInfo.Get();
        userInfo.isChoosePremiumWaifu = false;
        userInfo.selectedWaifuId = girlID;
        storageUserInfo.Save();
    }

    public static List<ModelChatCellViewContentOverview> GetListContentChat()
    {
        List<ModelChatCellViewContentOverview> listData = new();
        var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;

        var currCharRank = (int)gameInfo.CurrentCharRank;
        string messageRank = "";

        void ProcessForPremiumChar()
        {
            var apiUpgradeInfo = FactoryApi.Get<ApiUpgrade>().Data;
            for (int i = 0; i < apiUpgradeInfo.premium_waifu.Count; i++)
            {
                var ele = apiUpgradeInfo.premium_waifu[i];
                messageRank = ExtensionEnum.ToMessageCharPremium(gameInfo.CurrentGirlId, ele.level);
                listData.Add(new ModelChatCellViewContentOverview()
                {
                    IsGirlAvatar = true,
                    GirlID = ele.waifu_id,
                    Message = messageRank,
                    GirlName = ele.GetCharPremium().name,
                    ReachAtLevel = ele.level,
                    IsPremiumChar = true
                });
            }
        }

        for (int i = 1; i < currCharRank; i++)
        {
            var typeRank = (TypeLeagueCharacter)i;
            var dataRank = DBM.Config.rankingConfig.GetRankData(typeRank);
            messageRank = ExtensionEnum.ToMessage(dataRank.girlId, GameConsts.MAX_LEVEL_PER_CHAR - 1);
            listData.Add(new ModelChatCellViewContentOverview()
            {
                IsGirlAvatar = true,
                GirlID = dataRank.girlId,
                Message = messageRank,
                GirlName = dataRank.girlName,
                ReachAtLevel = GameConsts.MAX_LEVEL_PER_CHAR,
            });
        }

        int modLevel = gameInfo.current_level_girl % GameConsts.MAX_LEVEL_PER_CHAR;
        messageRank = ExtensionEnum.ToMessage(gameInfo.CurrentGirlId, modLevel);

        string girlName = DBM.Config.rankingConfig.GetRankDataBasedGirlId(gameInfo.CurrentGirlId).girlName;
        // Debug.LogError("Name : "+girlName);

        listData.Add(new ModelChatCellViewContentOverview()
        {
            IsGirlAvatar = true,
            GirlID = gameInfo.CurrentGirlId,
            Message = messageRank,
            GirlName = girlName,
            ReachAtLevel = modLevel + 1,
        });

        ProcessForPremiumChar();

        return listData;
    }

    public static List<ModelApiQuestData> GetQuestEventList(List<ModelApiQuestData> quests, string eventId,
        string tagName = "")
    {
        var groupQuest = quests.Where(x => x.IsQuestEvent(eventId))
            .OrderByDescending(x => x.can_claim)
            .ThenBy(x => x.claimed).ToList();

        return groupQuest;
    }

    public static List<ModelApiQuestData> GetQuestPartnerEventList(List<ModelApiQuestData> quests,
        ModelApiEventConfig partnerData)
    {
        // bool isMatchCondition = true;
        // if (partnerData.has_private)
        // {
        //     var apiUser = FactoryApi.Get<ApiUser>().Data;
        //     isMatchCondition = apiUser.User.IsHavePrivatePartner(partnerData.GetPartnerTagPrivate);
        // }

        var groupQuest = quests.Where(x =>
            {
                bool isMatchCondition = x.IsQuestEvent(partnerData.id);
                if (partnerData.IsHavePrivate() && x.event_tag?.Count > 0)
                {
                    // Debug.LogError("Tag Name : " + partnerData.tag_private[0]);
                    var apiUser = FactoryApi.Get<ApiUser>().Data;
                    var tag = partnerData.tag_private[0];
                    bool isHaveTag = apiUser.User.IsHavePrivatePartner(tag);

                    // Debug.LogError("Is Have Tag : " + isHaveTag + ", match Tag: " + x.IsMatchTagEvent(tag) +
                    //                ", match Condition: " + isMatchCondition);

                    return isHaveTag && x.IsMatchTagEvent(tag) && isMatchCondition;
                }

                return isMatchCondition;
            })
            .OrderByDescending(x => x.can_claim)
            .ThenBy(x => x.claimed).ToList();

        return groupQuest;
    }


    public static bool CanEndEventMeetYuki()
    {
        var apiQuest = FactoryApi.Get<ApiQuest>().Data;
        bool isDoneAllQuest = apiQuest.IsDoneAllQuestEventYuki();
        var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
        bool isReachChar3 = gameInfo.CurrentCharRank >= TypeLeagueCharacter.Char_3;
        var isClaimedBgYuki = FactoryApi.Get<ApiUpgrade>().Data.IsClaimedBgYuki;
        return isDoneAllQuest && isReachChar3 && isClaimedBgYuki;
        // return isReachChar3 && isClaimedBgYuki
    }

    public static bool IsMaintainance()
    {
// #if PRODUCTION_BUILD && !TEST_PRODUCTION_BUILD
        var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
        var apiCommon = FactoryApi.Get<ApiCommon>().Data.ServerInfo;

        if (gameInfo.is_maintenance && !apiCommon.IsMatchWl(MyTelegramId))
        {
            // var popup = UIPopup.Get(UIId.UIPopupName.PopupMaintenance.ToString());
            // popup.Show();
            return true;
        }
// #endif

        return false;
    }

    public static async UniTask CheckFollowXQuest(ModelApiQuestData data, UIPopup popup = null)
    {
        if (string.IsNullOrEmpty(data.link)) return;

        var apiQuest = FactoryApi.Get<ApiQuest>();
        var result = await apiQuest.Check(data.id);

        if (result.is_completed)
        {
            data.ReadyToClaim();
            apiQuest.Data.Sync(data);
            if (popup != null)
                popup.Hide();
        }
        else
        {
            var timeRemain = ServiceTime.GetTimeRemain(result.end_time);
            if (!data.is_verifying && timeRemain > 0)
            {
                data.end_time = result.end_time;
                apiQuest.Data.Sync(data);
            }

            GameUtils.OpenLink(data.link);
            if (popup != null)
                popup.Hide();
        }
    }

    public static int GetLevelWaifu(int girlId)
    {
        var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
        if (userInfo.isChoosePremiumWaifu)
        {
            return FactoryApi.Get<ApiUpgrade>().Data.GetPremiumChar(girlId).level - 1;
        }
        else
        {
            var apiGame = FactoryApi.Get<ApiGame>().Data.Info;
            return apiGame.current_level_girl;
        }
    }

    public static bool IsMatchTagEvent(ModelApiEventConfig eventConfig)
    {
        if (!eventConfig.IsHavePrivate())
            return false;
        
        var apiUser = FactoryApi.Get<ApiUser>().Data.User;
        return apiUser.IsHavePrivatePartner(eventConfig.tag_private[0]);
    }

    public static void ProcessSelectTapEffect(int id,Action onComplete=null)
    {
        var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
        var userInfo = storageUserInfo.Get();
        userInfo.selectedTapEffectId = id;

        storageUserInfo.Save();
        onComplete?.Invoke();
        // this.PostEvent(TypeGameEvent.ChangeTapEffect, true);
        // OnChangeTapEffect?.Invoke();
        ControllerPopup.ShowToastSuccess("Change effect successfully");
    }
}
