using System;
using System.Collections.Generic;
using BreakInfinity;
using Newtonsoft.Json;
using Template.Defines;
using UnityEngine.Serialization;

namespace Game.Model
{
	[Serializable]
	public class ModelApiQuestData
	{
		public string id;
		public string description;
		public string event_data;
		public string link;
		[JsonProperty("type")]
		public string category;
		public List<ModelApiItemData> items;
		public int achievement_index;
		public int process;
		public int processed;
		public bool can_claim;
		public bool claimed;
		public List<string> server_data;

		public string event_id;

		public long end_time;
		public string quest_event_check;

		public bool is_verifying;
		public List<string> event_tag;

		public QuestType Type => GetQuestType();

		public QuestCategory Category => GetQuestCategory();

		public bool QuestIsVerifying => end_time > 0;

		public void ReadyToClaim()
		{
			can_claim = true;
			claimed = false;
			processed = process;
			end_time = 0;
		}

		public void Claim()
		{
			can_claim = false;
			claimed = true;
			processed = process;
			end_time = 0;
		}

		public bool IsQuest()
		{
			var category = GetQuestCategory();
			return category == QuestCategory.Basic || category == QuestCategory.Daily ||
				   category == QuestCategory.Telegram;
		}

		public bool IsJackpotQuest()
		{
			return Type == QuestType.CheckInJackpot;
		}

		public bool IsQuestX()
		{
			var category = GetQuestCategory();
			return category == QuestCategory.X;
		}
		
		public bool IsQuestPartner()
		{
			var category = GetQuestCategory();
			return category == QuestCategory.Partner;
		}

		public bool IsAchievement()
		{
			var category = GetQuestCategory();
			return category == QuestCategory.Checkin || category == QuestCategory.Purchase ||
				   category == QuestCategory.Invite || category == QuestCategory.InvitePremium;
		}

		public bool IsNotcoinCollab()
		{
			var category = GetQuestCategory();
			return category == QuestCategory.NotcoinCollab;
		}

		// public bool IsQuestPartner()
		// {
		// 	if (event_data.StartsWith("partner_check_in"))
		// 	{
		// 		return event_data.Replace("partner_check_in_", "");
		// 	}
		//
		// 	return string.Empty;
		// }

		public bool IsQuestEvent(string eventId)
		{
			var category = GetQuestCategory();
			return category == QuestCategory.Event && event_id.Equals(eventId);
		}

		public bool IsMatchTagEvent(string tagName)
		{
			if (event_tag?.Count > 0)
			{
				if (event_tag.Exists(x => x.Equals(tagName)))
					return true;
			}

			return false;
		}

		private QuestType GetQuestType()
		{
			if (Enum.TryParse(event_data.SnakeToPascal(), true, out QuestType questType))
			{
				return questType;
			}

			if (event_data.StartsWith("partner_check_in"))
			{
				return QuestType.PartnerCheckIn;
			}

			return description switch
			{
				"Join Announcement" => QuestType.JoinAnnouncement,
				"Boost Channel" => QuestType.BoostChannel,
				_ => event_data switch
				{
					"share_story" => QuestType.ShareStory,
					"watch_ads" => QuestType.WatchAds,
					_ => QuestType.Common,
				},
			};
		}

		private QuestCategory GetQuestCategory()
		{
			if (Enum.TryParse(category.SnakeToPascal(), true, out QuestCategory questCategory))
			{
				return questCategory;
			}

			return QuestCategory.None;
		}
	}
}
