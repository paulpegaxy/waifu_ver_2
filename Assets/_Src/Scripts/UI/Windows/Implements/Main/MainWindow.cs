
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using UnityEngine;
using Game.Runtime;
using Game.Model;
using Game.Extensions;
using Template.Defines;
using Doozy.Runtime.UIManager.Containers;

namespace Game.UI
{
	public class MainWindow : UIWindow
	{
		[SerializeField] private GameObject ObjectAirdropGold;
		[SerializeField] private bool isTurnOffAllTrigger;

		protected override void OnEnabled()
		{
			this.RegisterEvent(TypeGameEvent.GameStart, OnGameStart);
			ButtonMainWindowAction.OnAction += OnAction;
		}

		protected override void OnDisabled()
		{
			this.RemoveEvent(TypeGameEvent.GameStart, OnGameStart);
			ButtonMainWindowAction.OnAction -= OnAction;
		}


		private void OnGameStart(object data)
		{
			// var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
			// if (!userInfo.isCustomizedProfile)
			// {
			// 	Signal.Send(StreamId.UI.CustomProfile);
			// }
			if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.GameFeature))
			{
				return;
			}

#if UNITY_EDITOR
			if (isTurnOffAllTrigger)
			{
				return;
			}
#endif

			if (FactoryApi.Get<ApiGame>().Data?.Info.rate_tap_bonus > 1)
				ControllerPopup.ShowInformation(
					"Celebrating 200K with 4x Sugar per Tap, live now till Nov 21, 08:00 (UTC)!");
			TriggerIdleEarnOffline();
			TriggerOfferShop();
			TriggerOfferHalloween();
			TriggerJackpotReward();
			ShowSubscription();
		}

		private void OnAction(MainWindowAction action)
		{
			switch (action)
			{
				case MainWindowAction.EventMeetYuki:
					break;
				case MainWindowAction.LimitedOffer:
					var offers = UIPopup.Get(UIId.UIPopupName.PopupOffer.ToString());
					UIPopup.AddPopupToQueue(offers);
					break;
				case MainWindowAction.Offer24Hour:
					SpecialExtensionShop.ShowPopupOfferDaySpecial().Forget();
					break;
				case MainWindowAction.Jackpot:
					Signal.Send(StreamId.UI.Jackpot);
					break;
				default:
					if (action.ToString().StartsWith("Partner"))
					{
						this.PostEvent(TypeGameEvent.Partner, action);
						Signal.Send(StreamId.UI.OpenPartner);
					}
					break;
			}
		}
		
		private async void ShowSubscription()
		{
			// var storageSetting = FactoryStorage.Get<StorageSetting>();
			// var model = storageSetting.Get();
			// if (model.DontShowSubscription) return;
			//
			// var apiUser = FactoryApi.Get<ApiUser>();
			// var subscription = await apiUser.GetSubscription();
			// if (subscription.telegram_subscription) return;
			//
			// var popup = UIPopup.Get(UIId.UIPopupName.PopupSubscription.ToString());
			// popup.GetComponent<PopupSubscription>().SetData(subscription);
			// UIPopup.AddPopupToQueue(popup);
		}

		private void TriggerIdleEarnOffline()
		{
			var apiGame = FactoryApi.Get<ApiGame>();
			if (apiGame.Data.IsHaveProfitFromOffline())
			{
				if (TutorialMgr.Instance.CategoryPlaying != TutorialCategory.NextGirl && TutorialMgr.Instance.CategoryPlaying != TutorialCategory.Undress)
				{
					var popup = UIPopup.Get(UIId.UIPopupName.PopupIdleEarning.ToString());
					popup.GetComponent<PopupIdleEarning>().SetData(apiGame.Data.IdleEarning);
					UIPopup.AddPopupToQueue(popup);
				}
			}
		}

		private async void TriggerJackpotReward()
		{
			// if (!GameFeature.IsSupport(GameFeatureType.Ton)) return;

			var storageSetting = FactoryStorage.Get<StorageSettings>();
			var model = storageSetting.Get();

			if (model.dontShowJackpot) return;

			var apiEvent = FactoryApi.Get<ApiEvent>();
			var jackpot = await apiEvent.Jackpot();
			if (jackpot.open_date > ServiceTime.CurrentUnixTime) return;

			var popup = UIPopup.Get(UIId.UIPopupName.PopupJackpot.ToString());
			popup.GetComponent<PopupJackpot>().SetData(jackpot);
			UIPopup.AddPopupToQueue(popup);
		}

		private async void TriggerOfferShop()
		{
			var apiShop = FactoryApi.Get<ApiShop>();

			var itemOfferLevelUp = apiShop.Data.GetItemsByPack(TypeShopPack.LevelOfferPack);
			if (itemOfferLevelUp?.Count > 0)
			{
				if (TutorialMgr.Instance.CategoryPlaying != TutorialCategory.NextGirl)
				{
					bool isActive = FactoryApi.Get<ApiShop>().Data.GetItemsByPack(TypeShopPack.LevelOfferPack).Any(x => !x.IsReachLimit);
					if (isActive)
						await SpecialExtensionShop.ShowPopupOfferLevelUp();
				}
			}

			SpecialExtensionShop.ShowPopupOfferDaySpecial().Forget();
			SpecialExtensionShop.ShowPopupHalloweenTap().Forget();
		}

		private void TriggerOfferHalloween()
		{
			// SpecialExtensionShop.ShowPopupUpOfferEventBundle();
		}
	}
}