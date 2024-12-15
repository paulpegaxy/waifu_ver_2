using System;
using AlmostEngine.Screenshot;
using Game.UI;
using Template.Defines;
using UnityEngine;

public static class ExtensionEnum
{
	// public static Color ToColor(this TypeLeague type)
	// {
	// 	return type switch
	// 	{
	// 		TypeLeague.Bronze => Utils.GetColor("#F4A368"),
	// 		TypeLeague.Silver => Utils.GetColor("#E2DED5"),
	// 		TypeLeague.Gold => Utils.GetColor("#FFC743"),
	// 		TypeLeague.Platinum => Utils.GetColor("#64CAFF"),
	// 		TypeLeague.Diamond => Utils.GetColor("#1FD86F"),
	// 		TypeLeague.Master => Utils.GetColor("#FF6369"),
	// 		TypeLeague.Royal => Utils.GetColor("#FF79FF"),
	// 		_ => Color.white,
	// 	};
	// }

	public static string ToBoosterName(this TypeBooster type)
	{
		return type switch
		{
			TypeBooster.MULTI_TAP => Localization.Get(TextId.Booster_MultiTap),
			TypeBooster.ENERGY_LIMIT => Localization.Get(TextId.Booster_EnergyLimit),
			TypeBooster.ENERGY_RESTORE => Localization.Get(TextId.Booster_EnergyFull),
			_ => "Undefined"
		};
	}

	public static string ToIdleEarnName(string id)
	{
		var textIdType = ("Idleearn_IdleEarn" + id).ToEnum(TextId.None);
		if (textIdType == TextId.None)
			return "Undefined";

		return Localization.Get(textIdType);
	}

	public static string ToMessage(int girlID, int level)
	{
		int modLevel = (level + 1) % (GameConsts.MAX_LEVEL_PER_CHAR + 1);

		// var textIdType = ($"Messagegirl_{type}Char{girlNumber}lv{modLevel}").ToEnum(TextId.None);
		// if (textIdType == TextId.None)
		if (modLevel == 0)
			return "Please Next Girl";

		// Debug.LogError("Text final: " + Localization.Get(textIdType) + "Type: " + textIdType);

		return Localization.Get($"MESSAGEGIRL/REGULAR_{girlID}{modLevel}");
	}
	
	public static string ToMessageCharPremium(int girlID, int level)
	{
		return Localization.Get($"MESSAGEGIRL/REGULAR_{girlID}{level}");
	}

	public static string ToRewardGirlMessage(int girlID)
	{
		return Localization.Get($"MESSAGEGIRL/REWARD_{girlID}");
	}

	public static string ToQuestLocalize(string questId)
	{
		return Localization.Get($"QUESTCONFIG/QUEST_{questId}");
	}

	public static string ToEventBundleName(TypeEventBundleCellView type,string eventId)
	{
		string eventName = eventId.ToUpperCase();
		switch (type)
		{
			case TypeEventBundleCellView.PackBg:
				return Localization.Get($"EVENT/{eventName}_BG");
			case TypeEventBundleCellView.PackTap:
				return Localization.Get($"EVENT/{eventName}_TAP");
			default:
				return "";
		}
	}

	public static string ToQuestTitleTotal(this QuestCategory type)
	{
		return type switch
		{
			QuestCategory.Checkin => Localization.Get(TextId.Quest_TotalCheckin),
			QuestCategory.Purchase =>  Localization.Get(TextId.Quest_TotalPurchase),
			QuestCategory.Invite | QuestCategory.InvitePremium =>  Localization.Get(TextId.Quest_TotalInvite),
			_ => Localization.Get(TextId.Common_Total)
		};
	}

	public static string ToQuestAchievementHeader(this QuestCategory type)
	{
		return type switch
		{
			QuestCategory.Checkin => Localization.Get(TextId.Quest_BonusCheckIn),
			QuestCategory.Purchase => Localization.Get(TextId.Quest_BonusPurchase),
			QuestCategory.Invite => Localization.Get(TextId.Friend_LbInviteFriends),
			QuestCategory.InvitePremium => Localization.Get(TextId.Quest_InviteFriendsPremium),
			_ => Localization.Get(TextId.Quest_Achievement)
		};
	}

	public static TypeResource ToResourceType(this string id)
	{
		int intId = int.Parse(id);
		return (TypeResource)intId;
	}
	
	public static TEnum ToEnum<TEnum>(this string strEnumValue)
	{
		if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
			throw new Exception($"Can not parse {strEnumValue} to enum: {nameof(TEnum)}");

		return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
	}

	public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
	{
		if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
			return defaultValue;

		return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
	}

	public static bool CanParseToEnum<TEnum>(this int intEnumValue)
	{
		if (Enum.IsDefined(typeof(TEnum), intEnumValue))
			return true;

		return false;
	}

}