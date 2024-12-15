using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Debug;
using Game.Model;
using Template.Defines;
using Unity.VisualScripting;

namespace Game.Runtime
{
	[Factory(ApiType.Game, true)]
	public class ApiGame : Api<ModelApiGame>
	{
		public async UniTask<ModelApiGameLogin> Login(string telegram_token)
		{
			var login = await Post<ModelApiGameLogin>("/v1/login/login-by-telegram", "data", new { telegram_token });
			GameUtils.Log("red", "[access_token] " + login.access_token);
			Data.Login = login;
			return login;
		}

		public async UniTask<ModelApiGameLogin> RefreshToken()
		{
			var refresh_token = Data.Login.refresh_token;
			var login = await Post<ModelApiGameLogin>("/v1/login/refresh-token", "data", new { refresh_token });

			Data.Login = login;
			Data.Login.refresh_token = login.refresh_token;

			return login;
		}

		public async UniTask<ModelApiGameInfo> GetInfo(bool isForceSync = true)
		{
			var info = await Get<ModelApiGameInfo>("/v1/game/info", "data");
			Data.IdleEarning = info.idle_earnings;

			SyncGameInfo(info, isForceSync: isForceSync);

			return info;
		}

		// public async UniTask<ModelApiGameInfoNew> GetInfoNew()
		// {
		// 	var info = new ModelApiGameInfoNew();
		// 	info.chat_point = 100;
		// 	info.price_send_chat = 2;
		// 	info.swipe_count = GameConsts.MAX_SWIPE_COUNT;
		// 	SyncGameInfoNew(info);
		// 	return info;
		// }

		// public async void PostMatchGirl()
		// {
		// 	Data.InfoNew.swipe_count--;
		// 	Data.InfoNew.Notification();
		// }
		//
		// public async UniTask PostSendChatGirl()
		// {
		// 	Data.InfoNew.chat_point -= Data.InfoNew.price_send_chat;
		// 	SyncGameInfoNew(Data.InfoNew);
		// }

		// private void SyncGameInfoNew(ModelApiGameInfoNew info)
		// {
		// 	Data.InfoNew = info;
		// 	Data.InfoNew.Notification();
		// }
		
		public async UniTask PostTutorialStep(TutorialCategory tutorialCategory)
		{
			int index = (int)tutorialCategory;
			var data=await Post<ModelApiGameInfoCachePost>("/v1/game/info", "data", new { tutorial_step = index.ToString() });
			SyncManualGameInfo(data);
		}

		public async UniTask<ModelApiGameInfo> PostTapCount(int tapCount)
		{
			var data = await Post<ModelApiGameInfoCachePost>("/v1/game/tap", "data", new {tap_counts = tapCount});
			SyncManualGameInfo(data, false);
			return Data.Info;
		}

		public async UniTask PostChargeStamina()
		{
			var data = await Post<ModelApiGameInfoCachePost>("/v1/game/charge-stamina", "data");
			GetInfo().Forget();
		}

		public async UniTask PostUpgradeStaminaMax()
		{
			var data=await Post<ModelApiGameInfoCachePost>("/v1/game/upgrade-energy-limit", "data");
			GetInfo().Forget();
		}

		public async UniTask PostUpgradeMultiTap()
		{
			var data=await Post<ModelApiGameInfoCachePost>("/v1/game/upgrade-multi-tap", "data");
			GetInfo().Forget();
		}

		public async UniTask PostIncreaseLevel()
		{
			await Post("/v1/game/upgrade-level-girl", new { });
			GetInfo().Forget();
		}

		public async UniTask IdleClaimFree()
		{
			await Post<bool>("/v1/game/idle-free", "status", new { });
			GetInfo().Forget();
		}

		public async UniTask IdleClaimPremium()
		{
			await Post<bool>("/v1/game/idle-premium", "status", new { });
			GetInfo().Forget();
		}

		private void SyncGameInfo(ModelApiGameInfo info, bool notification = true, bool isForceSync = true)
		{
			Data.Info = info;
			ControllerResource.Set(TypeResource.Ton, info.Ton);
			if (notification)
			{
				Data.Info.Notification();
			}

			if (isForceSync)
				ServiceLocator.GetService<IServiceSyncData>().ForceSyncData(Data.Info);
		}

		private void SyncManualGameInfo(ModelApiGameInfoCachePost data, bool isForceSync = true)
		{
			data.ConvertSave(ref Data.Info);
			Data.Info.Notification();
			if (isForceSync)
				ServiceLocator.GetService<IServiceSyncData>().ForceSyncData(Data.Info);
		}

		private async UniTask<ModelApiGameInfo> Post(string path, object data)
		{
			var info = await Post<ModelApiGameInfo>(path, "data", data);
			await GetInfo();
			// SyncGameInfo(info);
			return info;
		}


		#region CHEAT GAME

		private const string endPointCheat = "/v1/game/cheat";

		public async void CheatPoint(int value = 1000000)
		{
			var point = Data.Info.PointParse + value;
			var point_all_time = Data.Info.PointAllTimeParse + value;
			await Post(endPointCheat, new { point = point.ToString(), point_all_time = point_all_time.ToString() });
		}

		public async void CheatBerry(int value = 1000)
		{
			var berry = Data.Info.BerryParse + value;
			var berryAllTime = Data.Info.BerryAllTimeParse + value;
			await Post(endPointCheat, new { berry = berry.ToString(), point_all_time = berryAllTime.ToString() });
		}

		public async UniTask CheatZeroStamina()
		{
			await Post(endPointCheat, new { stamina = 0 });
		}

		public async UniTask ResetData()
		{
			await Post("/v1/game/reset", new { });
		}

		public async UniTask CheatAddPartner(List<string> partner)
		{
			await Post(endPointCheat, new { tags = partner });
		}
		#endregion

	}
}