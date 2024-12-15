using System;
using System.Linq;
using System.Collections.Generic;
using AlmostEngine.SimpleLocalization;
using UnityEngine;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
	public class QuestWindow : UIWindow
	{
		[SerializeField] private QuestScroller scrollerQuest;
		[SerializeField] private QuestScroller scrollerQuestX;
		[SerializeField] private QuestScroller scrollerQuestPartner;
		[SerializeField] private QuestScroller scrollerAchievement;

		protected override void OnEnabled()
		{
			Fetch();
			ModelApiQuest.OnChanged += OnChanged;
			QuestExecute.OnRefresh += OnRefresh;
			this.RegisterEvent(TypeGameEvent.ClaimAchievementSuccess, OnRefreshAfterAchievement);
		}

		protected override void OnDisabled()
		{
			ModelApiQuest.OnChanged -= OnChanged;
			QuestExecute.OnRefresh -= OnRefresh;
			this.RemoveEvent(TypeGameEvent.ClaimAchievementSuccess, OnRefreshAfterAchievement);
		}
		
		private void OnChanged(ModelApiQuest data)
		{
			scrollerQuest.ReloadData();
			scrollerQuestX.ReloadData();
			scrollerQuestPartner.ReloadData();
		}

		private void OnRefreshAfterAchievement(object data)
		{
			OnRefresh();
		}
		
		private void OnRefresh()
		{
			Fetch();
		}
		
		private async void Fetch()
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiQuest = FactoryApi.Get<ApiQuest>();
				// UnityEngine.Debug.LogError("Reload request");
				var info = await apiQuest.Get();
				Process(info);
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);
		}

		private async void GetQuests()
		{
			this.ShowProcessing();
			try
			{
				var apiQuest = FactoryApi.Get<ApiQuest>();
				var info = await apiQuest.Get();
				Process(info);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private void Process(ModelApiQuestInfo info)
		{
			var groupQuest = info.quests.Where(x => x.IsQuest())
				.OrderByDescending(x=>x.IsJackpotQuest())
				.ThenByDescending(x => x.can_claim)
				.ThenBy(x => x.claimed).ToList();

			var groupQuestX = info.quests.Where(x => x.IsQuestX())
				.OrderByDescending(x => x.can_claim)
				.ThenBy(x => x.claimed).ToList();
			
			var groupQuestPartner=info.quests.Where(x=>x.IsQuestPartner())
				.OrderByDescending(x => x.can_claim)
				.ThenBy(x => x.claimed).ToList();
			
			var dataQuest = new List<ModelQuestCellView>();
			foreach (var quest in groupQuest)
			{
				if (quest.IsJackpotQuest())
					dataQuest.Add(new ModelQuestCellViewContentQuestJackpot() { Quest = quest });
				else
					dataQuest.Add(new ModelQuestCellViewContentQuest() { Quest = quest });
			}

			var dataQuestX = new List<ModelQuestCellView>();
			foreach (var quest in groupQuestX)
			{
				dataQuestX.Add(new ModelQuestCellViewContentQuest() { Quest = quest });
			}

			var dataQuestPartner = new List<ModelQuestCellView>();
			foreach (var quest in groupQuestPartner)
			{
				dataQuestPartner.Add(new ModelQuestCellViewContentQuest() { Quest = quest });
			}
			
			scrollerQuest.SetData(dataQuest);
			scrollerQuestX.SetData(dataQuestX);
			scrollerQuestPartner.SetData(dataQuestPartner);

			LoadAchievementContent(info);
		}

		private void LoadAchievementContent(ModelApiQuestInfo info)
		{
			var groupAchievement = info.quests.Where(x => x.IsAchievement()).GroupBy(x => x.Category).OrderBy(x => (int)x.Key);
			
			var dataAchievement = new List<ModelQuestCellView>();
			var totals = new Dictionary<QuestCategory, float>()
			{
				{ QuestCategory.Checkin, info.achievement_summary.total_checkin },
				{ QuestCategory.Purchase, info.achievement_summary.total_purchase_usd },
				{ QuestCategory.Invite, info.achievement_summary.total_invite_normal_friends },
				{ QuestCategory.InvitePremium, info.achievement_summary.total_invite_premium_friends },
			};

			foreach (var item in groupAchievement)
			{
				dataAchievement.Add(new ModelQuestCellViewHeaderAchievement()
				{
					QuestCategory = item.Key,
					Title = GetTitle(item.Key),
					TotalCompleted = totals[item.Key],
				});

				var items = item.ToList();
				for (var i = 0; i < items.Count; i++)
				{
					items[i].achievement_index = i + 1;
				}
				dataAchievement.Add(new ModelQuestCellViewContentAchievement() { Quests = items });
			}
			scrollerAchievement.SetData(dataAchievement);
		}

		private string GetTitle(QuestCategory type)
		{
			var textIds = new Dictionary<QuestCategory, TextId>()
			{
				{QuestCategory.Daily, TextId.Quest_Daily},
				{QuestCategory.Basic, TextId.Quest_Basic},
				{QuestCategory.X, TextId.Quest_XTask},
				{QuestCategory.Telegram, TextId.Quest_Telegram},
				{QuestCategory.Checkin, TextId.Quest_CheckIn},
				{QuestCategory.Purchase, TextId.Quest_Purchase},
				{QuestCategory.Invite, TextId.Quest_Friend},
				{QuestCategory.InvitePremium, TextId.Quest_PremiumFriend},
				{QuestCategory.Partner, TextId.Quest_Partner},
			};

			return Localization.Get(textIds[type]);
		}
	}
}