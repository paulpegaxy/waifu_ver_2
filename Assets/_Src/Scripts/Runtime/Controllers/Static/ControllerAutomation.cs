using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager;
using Game.Extensions;
using Template.Defines;
using UnityEngine;

namespace Game.Runtime
{
	public static class ControllerAutomation
	{
		private static CancellationTokenSource _cts;

		private static bool _isUsePremium;

		public static int AutoTapCount;
		public static bool IsStarted;


		public static void Start(bool isUsePremium)
		{
			if (IsStarted) return;

			AutoTapCount = 0;
			_cts?.Cancel();
			_cts = new CancellationTokenSource();
			IsStarted = true;
			_isUsePremium = isUsePremium;

			PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(1f), true, DelayType.DeltaTime, PlayerLoopTiming.Update, _cts.Token, Automation, null);
		}

		public static void Stop()
		{
			if (!IsStarted) return;
			AutoTapCount = 0;
			_isUsePremium = false;
			_cts?.Cancel();
			IsStarted = false;
		}

		private static void Automation(object data)
		{
			if (SpecialExtensionGame.IsAutoBotPurchased())
				ProcessAutoPremium();
			else
				ProcessAutoTrial();
		}

		private static void AutoRefillEnergy()
		{
			if (SpecialExtensionGame.CanRestoreStamina())
			{
				var staminaValue = ControllerResource.Get(TypeResource.ExpWaifu).Amount;
				var pointPerTap =  FactoryApi.Get<ApiGame>().Data.Info.PointPerTapParse;

				var multipleValue = pointPerTap * GameConsts.BOT_PREMIUM_AUTO_TAP;
				if (staminaValue < multipleValue)
				{
					GameUtils.Log("blue", "AutoRefillEnergy");
					FactoryApi.Get<ApiGame>().PostChargeStamina().Forget();
					FactoryApi.Get<ApiUpgrade>().Get().Forget();
					ControllerPopup.ShowToastSuccess("Your energy is filled");
				}
			}
		}

		// private static void AutoTap(int tapPerSecond)
		// {
		// 	var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
		// 	var staminaValue = ControllerResource.Get(TypeResource.Stamina).Amount;
		// 	var pointPerTap = gameInfo.PointPerTapParse;
		//
		// 	var multipleValue = pointPerTap * tapPerSecond;
		// 	if (staminaValue < multipleValue)
		// 	{
		// 		// UnityEngine.Debug.LogError("Vao day " + staminaValue + ", value check " + multipleValue +
		// 		//                            " autoCount: " + AutoTapCount);
		// 		return;
		// 	}
		//
		// 	AutoTapCount += tapPerSecond;
		// 	SpecialExtensionObserver.PostEvent(null, TypeGameEvent.BotSendAutoTap, tapPerSecond);
		// }

		private static void AutoTap(int tapPerSecond)
		{
			var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
			var staminaValue = ControllerResource.Get(TypeResource.ExpWaifu).Amount;
			var pointPerTap = gameInfo.PointPerTapParse;

			var multipleValue = pointPerTap * tapPerSecond;

			// UnityEngine.Debug.LogError("multipleValue: " + multipleValue + " staminaValue: " + staminaValue);
			
			if (staminaValue < multipleValue)
			{
				// var value = staminaValue / pointPerTap;
				// if (value > 0)
				// {
				// 	int finalTap = Mathf.FloorToInt(float.Parse(value.ToString()));
				// 	AutoTapCount += finalTap;
				// 	SpecialExtensionObserver.PostEvent(null, TypeGameEvent.BotSendAutoTap, finalTap);
				// }
			}
			else
			{
				AutoTapCount += tapPerSecond;
				SpecialExtensionObserver.PostEvent(null, TypeGameEvent.BotSendAutoTap, tapPerSecond);
			}
		}


		private static void ProcessAutoTrial()
		{
			AutoTap(GameConsts.BOT_TRIAL_AUTO_TAP);
		}

		private static void ProcessAutoPremium()
		{
			AutoTap(GameConsts.BOT_PREMIUM_AUTO_TAP);
			AutoRefillEnergy();
		}



		// public static void AutoClaimAirdrop(EntityType type)
		// {
		// 	var apiGame = FactoryApi.Get<ApiGame>();
		// 	if (type == EntityType.Cococopter)
		// 	{
		// 		apiGame.AirdropPetClaimFree().Forget();
		// 	}
		// 	else if (type == EntityType.Mushroom)
		// 	{
		// 		apiGame.AirdropSpeedClaimFree().Forget();
		// 	}
		// }
	}
}