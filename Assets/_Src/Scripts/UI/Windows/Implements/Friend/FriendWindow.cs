using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class FriendWindow : UIWindow
	{
		[SerializeField] private FriendScroller scrollerReferral;
		[SerializeField] private BuddyScroller scrollerBuddy;
		[SerializeField] private UIToggleGroup toggleGroup;
		[SerializeField] private UIButton buttonInformation;

		private void Awake()
		{
			buttonInformation.gameObject.SetActive(false);
		}

		protected override void OnEnabled()
		{
			toggleGroup.OnToggleTriggeredCallback.AddListener(OnTabChanged);
			BuddyCellViewHeader.OnClaim += OnClaim;
			PopupConfirmBundle.OnCompleted += OnBuySuccess;
			FriendCellViewContentFriendProgress.OnRefreshAfterSlap += OnRefreshAfterSlap;
			Fetch().Forget();
		}

		protected override void OnDisabled()
		{
			toggleGroup.OnToggleTriggeredCallback.RemoveListener(OnTabChanged);
			BuddyCellViewHeader.OnClaim -= OnClaim;
			PopupConfirmBundle.OnCompleted -= OnBuySuccess;
			FriendCellViewContentFriendProgress.OnRefreshAfterSlap -= OnRefreshAfterSlap;
		}



		private void OnTabChanged(UIToggle toggle)
		{
			// UnityEngine.Debug.LogError("Tab changed: " + toggleGroup.lastToggleOnIndex);
			var isFriend = toggleGroup.lastToggleOnIndex == 0;
			buttonInformation.gameObject.SetActive(!isFriend);
		}

		private void OnBuySuccess(ModelApiShopBuy data)
		{
			Fetch().Forget();
		}

		private void OnRefreshAfterSlap()
		{
			Fetch().Forget();
		}

		private async UniTask Fetch()
		{
			this.ShowProcessing();
			try
			{
				var apiFriend = FactoryApi.Get<ApiFriend>();
				var data = await apiFriend.Get();
				var friends = await apiFriend.GetFriends();
				var eventConfig = await apiFriend.EventConfig();

				ProcessReferral(data, friends, eventConfig);
				ProcessBuddyTree(data);

				OnDataLoaded?.Invoke(UIId.UIViewCategory.Window, UIId.UIViewName.Friend);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private async void OnClaim(ModelBuddyCellViewHeader data)
		{
			this.ShowProcessing();
			try
			{
				var apiFriend = FactoryApi.Get<ApiFriend>();
				await apiFriend.ClaimBuddyTree();
				await FactoryApi.Get<ApiGame>().GetInfo();
				await Fetch();

				this.HideProcessing();
				ControllerPopup.ShowToast(TextId.Toast_NotiClaimSuccess);
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private void ProcessReferral(ModelApiFriendConfig info, ModelApiFriendInvited friends, ModelApiFriendEventConfig eventConfig)
		{
			var data = new List<ModelFriendCellView>();
			if (eventConfig.HasEvent)
			{
				data.Add(new ModelFriendCellViewContentEventBanner() { Config = eventConfig.current });
			}
			data.AddRange(new List<ModelFriendCellView>
			{
				new ModelFriendCellViewContentTop()
				{
					TotalInvited = info.total_invited,
				},
				new ModelFriendCellViewHeader()
				{
					Title = Localization.Get(TextId.Friend_InviteToGetBonus),
				},
				new ModelFriendCellViewContentBonus()
				{
					NormalBonus = info.referral_invite_bonus_config[0].normal_friend,
					PremiumBonus = info.referral_invite_bonus_config[0].telegram_premium_friend,
				},

			});

			if (friends.data.Count > 0)
			{
				data.Add(new ModelFriendCellViewHeader()
				{
					Title = Localization.Get(TextId.Friend_List),
				});
			}

			foreach (var item in friends.data)
			{
				data.Add(new ModelFriendCellViewContentFriendProgress()
				{
					Name = item.user.name,
					FriendId = item.user.id,
					FriendCount = item.total_invited,
					Score = item.total_berry,
					CurrentGirlLevel = item.user.current_girl_level,
					DelaySlapTime = item.cooldown_slap,
					IsPremiumUser = item.user.is_premium,
					ConfigProgress = info.referral_friend_bonus_config
				});
			}

			scrollerReferral.SetData(data);
		}

		private void ProcessBuddyTree(ModelApiFriendConfig info)
		{
			var data = new List<ModelBuddyCellView>
			{
				new ModelBuddyCellViewHeader()
				{
					Processing = 0,
					Total = 0,
				},
			};

			var statistics = data[0] as ModelBuddyCellViewHeader;
			foreach (var item in info.referral_layer_config)
			{
				var unlockPrice = Mathf.Max(0, item.iap - info.total_usd_iap);
				var currentFriend = info.GetInvitedByLayer(item.layer);
				var berry = info.GetProfit(TypeResource.Berry, item.layer);
				var ton = info.GetProfit(TypeResource.Ton, item.layer);
				var isLocked = currentFriend < item.invite || info.total_usd_iap < item.iap;

				if (isLocked)
				{
					data.Add(new ModelBuddyCellViewContentNormal()
					{
						BuddyType = (BuddyType)item.layer,
						Commission = item.benefit,
						CurrentFriend = currentFriend,
						MaxFriend = item.invite,
						TotalSpend = info.total_usd_iap,
						MaxSpend = item.iap,
						HardCurrency = berry,
						Ton = ton,
						// UnlockPrice = unlockPrice % 1 != 0 ? unlockPrice + 1 : unlockPrice,
						UnlockPrice = unlockPrice
					});
				}
				else
				{
					data.Add(new ModelBuddyCellViewContentUnlocked()
					{
						BuddyType = (BuddyType)item.layer,
						Commission = item.benefit,
						CurrentFriend = currentFriend,
						MaxFriend = item.invite,
						TotalSpend = info.total_usd_iap,
						MaxSpend = item.iap,
						HardCurrency = berry,
						Ton = ton,
					});

					statistics.Total += ton;
				}

				statistics.Processing += ton;
			}

			scrollerBuddy.SetData(data);
		}
	}
}