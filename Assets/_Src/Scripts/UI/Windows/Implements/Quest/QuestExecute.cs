using System;
using UnityEngine;
using Doozy.Runtime.Signals;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
	public class QuestExecute : MonoBehaviour
	{
		public static Action OnRefresh;

		private float _refreshTime;

		private void OnEnable()
		{
			QuestCellViewContentQuest.OnGo += OnQuestGo;
			QuestCellViewContentQuest.OnClaim += OnQuestClaim;
		}

		private void OnDisable()
		{
			QuestCellViewContentQuest.OnGo -= OnQuestGo;
			QuestCellViewContentQuest.OnClaim -= OnQuestClaim;
		}

		private void Update()
		{
			if (_refreshTime > 0)
			{
				_refreshTime -= Time.deltaTime;
				if (_refreshTime <= 0)
				{
					_refreshTime = 0;
					OnRefresh?.Invoke();
				}
			}
		}

		private async void OnQuestGo(ModelQuestCellViewContentQuest data)
		{
			var apiQuest = FactoryApi.Get<ApiQuest>();
			var quest = data.Quest;
			var apiShop = FactoryApi.Get<ApiShop>();

			switch (quest.Type)
			{
				case QuestType.DailyCheckIn:
					ControllerPopup.SetApiLoading(true);
					try
					{
						// await apiShop.BuyWithToken(ShopItemType.CheckIn);
						await apiShop.BuyWithTon(TypeShopItem.CheckIn);

						_refreshTime = 1;
					}
					catch
					{
					}
					ControllerPopup.SetApiLoading(false);
					return;

				case QuestType.DailyPurchase:
				case QuestType.Purchase:
				case QuestType.WatchAds:
					Signal.Send(StreamId.UI.OpenShop);
					return;
				case QuestType.TonCheckInVerify:
					ControllerPopup.ShowToastError("Not qualified to claim! Please check again!");
					return;

				case QuestType.DailyInvite:
					SpecialExtensionGame.Invite();
					break;
				case QuestType.CheckInJackpot:
					await SpecialExtensionShop.BuyCheckIn(quest);
					_refreshTime = 1;
					break;
				case QuestType.UnlockWaifu:
				case QuestType.MeetYuki:
					Signal.Send(StreamId.UI.BackToMain);
					break;
				case QuestType.NameIcon:
#if UNITY_EDITOR
					GameUtils.CopyToClipboard("\ud83c\udf51$WIFE");
					#else
						TelegramWebApp.CopyToClipboard("\ud83c\udf51$WIFE");
#endif
			
				
					ControllerPopup.ShowToast("Copied Icon to clipboard");
					break;

				case QuestType.ShareStory:
					if (TelegramWebApp.IsMobile())
					{
						this.ShowProcessing();
						var configs = apiQuest.Data.Configs;
						if (configs == null)
						{
							await apiQuest.GetConfig();
						}

						var shareStory = configs.GetShareStory(quest.id);
						if (shareStory == null)
						{
							ControllerPopup.SetApiLoading(false);
							return;
						}

						var text = shareStory.description;
						var mediaUrl = shareStory.link;

						var status = SpecialExtensionGame.ShareToStory(text, mediaUrl);
						if (!status)
						{
							ControllerPopup.ShowToastError("Can not share story at this time");
						}
						else
						{
							_ = apiQuest.StoryCheck(data.Quest.id);
						}
						this.HideProcessing();
					}
					else
					{
						ControllerPopup.ShowToastError("Only use on mobile");
					}
					return;
			}

			if (string.IsNullOrEmpty(data.Quest.link)) return;

			var result = await apiQuest.Check(quest.id);

			if (result.is_completed)
			{
				quest.ReadyToClaim();
				apiQuest.Data.Sync(quest);
			}
			else
			{
				var timeRemain = ServiceTime.GetTimeRemain(result.end_time);
				if (!quest.is_verifying && timeRemain > 0)
				{
					quest.end_time = result.end_time;
					apiQuest.Data.Sync(data.Quest);
				}

				GameUtils.OpenLink(data.Quest.link);
			}

			// if (quest.is_verifying)
			// {
			// 	// UnityEngine.Debug.LogError("Recheck ne: "+quest.id);
			// 	var result = await apiQuest.Check(data.Quest.id);
			// 	if (result.is_completed)
			// 	{
			// 		apiQuest.Data.Claim(data.Quest);
			// 	}
			// 	else
			// 	{
			// 		if (result.end_time > 0)
			// 		{
			// 			var timeRemain = ServiceTime.GetTimeSpanEndData(result.end_time);
			// 			ControllerPopup.ShowInformation(
			// 				$"Quest is being verified. Please try again in {timeRemain.TimeSpanToString()}");
			// 		}
			// 		else
			// 		{
			// 			// GameUtils.OpenLink(data.Quest.link);
			// 			ControllerPopup.ShowToast("The task is being verified. Please be patient");
			// 		}
			// 	}
			// }
			// else
			// {
			// 	GameUtils.OpenLink(data.Quest.link);
			// 	var result = await apiQuest.Check(data.Quest.id);

			// 	if (result.end_time > 0)
			// 	{
			// 		var timeRemain = ServiceTime.GetTimeSpanEndData(result.end_time);
			// 		ControllerPopup.ShowInformation(
			// 			$"Quest is being verified. Please try again in {timeRemain.TimeSpanToString()}");
			// 	}
			// 	else
			// 	{
			// 		ControllerPopup.ShowInformation("The task is being verified. Please be patient");
			// 	}

			// 	_refreshTime = 5;
			// }

		}

		private async void OnQuestClaim(ModelQuestCellViewContentQuest data, Vector3 position)
		{
			var apiQuest = FactoryApi.Get<ApiQuest>();
			await apiQuest.Claim(data.Quest.id);

			foreach (var item in data.Quest.items)
			{
				ControllerResource.Add(item.IdResource, item.QuantityParse);
				ControllerUI.Instance.Spawn(item.IdResource, position, 20);
			}

			data.Quest.Claim();
			OnRefresh?.Invoke();
		}
	}
}