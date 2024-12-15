using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Nody;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Sirenix.OdinInspector;
using Game.Runtime;
using Template.Defines;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.UI
{
	public class InitWindow : MonoBehaviour
	{
		[SerializeField] private FlowController FlowController;
		[SerializeField] private InitLoadingBar LoadingBar;

		[SerializeField, Multiline(lines: 10)] private string defaultTestTelegramUserData;
		[ShowInInspector, MultiLineProperty(lines: 10)]
		private string TestTelegramUserData
		{
			get
			{
#if UNITY_EDITOR
				return EditorPrefs.GetString(nameof(TestTelegramUserData), defaultTestTelegramUserData);
#else
				return defaultTestTelegramUserData;
#endif
			}
#if UNITY_EDITOR
			set => EditorPrefs.SetString(nameof(TestTelegramUserData), value);
#endif
		}


		private enum State
		{
			None,
			Doozy,
			Load,
			Finished,
		}

		private State _state;

		private void Start()
		{
			ControllerAudio.Instance.Mute(AudioMixerType.Bgm);
			ControllerAudio.Instance.Mute(AudioMixerType.Sfx);

			// Application.targetFrameRate = model.quality == TypeQuality.Low ? 30 : 60;
			Application.targetFrameRate = 60;
			QualitySettings.vSyncCount = 0;
#if PRODUCTION_BUILD && !UNITY_EDITOR
            UnityEngine.Debug.unityLogger.logEnabled = false;
#endif

			SetState(State.Doozy);
		}

		private void Update()
		{
			switch (_state)
			{
				case State.Doozy:
					if (FlowController.initialized)
					{
						SetState(State.Load);
					}
					break;
			}
		}

		private void SetState(State state)
		{
			_state = state;

			switch (_state)
			{
				case State.Doozy:
					LoadingBar.SetProgress(0f);
					break;

				case State.Load:
					Load().Forget();
					break;

				case State.Finished:
					Signal.Send(StreamId.Game.Init);
					this.PostEvent(TypeGameEvent.GameStart);
					break;
			}
		}

		private async UniTask Load()
		{
			await ServiceTime.Init();

			var apiCommon = FactoryApi.Get<ApiCommon>();
			_ = apiCommon.GetSummary();

			SetLoadingText("Load game...");
			SetLoadingProgress(0.85f);

			GameUtils.Log("yellow", "-------------Load Audio-------------");
			LoadAudio().Forget();

			GameUtils.Log("yellow", "-------------Load Resource-------------");
			LoadResource();

			GameUtils.Log("yellow", "-------------Load Api-------------");
			await ApiInit();

			GameUtils.Log("green", "-------------Finish Load-------------");
			// this.PostEvent(TypeGameEvent.GameStart);
		}

		private void LoadResource()
		{
			AnR.LoadResourceByFolder<GameObject>("Common");
			// ControllerSpawner.Instance.PrePool(AnR.GetKey(AnR.CommonKey.VfxFloatingItem), GameConsts.MAX_LENGTH_PRE_POOL_FLOAT_ITEM_EFF);
			ControllerSpawner.Instance.PrePool(AnR.GetKey(AnR.CommonKey.VfxSelection), GameConsts.MAX_LENGTH_PRE_POOL_SELECTION_EFF);
			// ControllerSpawner.Instance.PrePool(AnR.GetKey(AnR.CommonKey.VfxTapGirl), GameConsts.MAX_LENGTH_PRE_POOL_FLOAT_ITEM_EFF);
		}

		private async UniTask LoadAudio()
		{
			await AnR.LoadAddressableByLabels<AudioClip>(new List<string>() { "audio" });
			// await AnR.LoadAddressableByLabels<AudioClip>(new List<string>() { "audiogirl" });
			// var listAudioGirl = AnR.GetAllAssetsByLabel<AudioClip>("audiogirl");
			// GameUtils.ShuffleList(ref listAudioGirl);
			// ControllerAudio.Instance.ProcessQueueAudioGirl(listAudioGirl);
		}

		private async UniTask ApiInit()
		{
			var apiGame = FactoryApi.Get<ApiGame>();
			await apiGame.Login(GetTelegramInitData());
			
			var apiCommon = FactoryApi.Get<ApiCommon>();
			
			var apiUser = FactoryApi.Get<ApiUser>();
			await apiUser.Get();
			await apiUser.GetSubscriptions();

			// apiUser.Data.User.telegram_id = "dasdsadsa";
			
// #if PRODUCTION_BUILD && !TEST_PRODUCTION_BUILD
			if (apiCommon.Data.ServerInfo.is_maintenance && !apiCommon.Data.ServerInfo.IsMatchWl(apiUser.Data.User.telegram_id))
			{
				var popup = UIPopup.Get(UIId.UIPopupName.PopupMaintenance.ToString());
				popup.Show();

				TelegramWebApp.SetLoadingProgress(1);
				return;
			}
// #endif

			// var apiUpgrade = FactoryApi.Get<ApiUpgrade>();
			// await apiUpgrade.Get();

			// var apiLeaderboard = FactoryApi.Get<ApiLeaderboard>();
			// await apiLeaderboard.GetConfig();
			// LoadConfigEditor(apiLeaderboard.Data.Config.ConfigPersonal);

			// await apiGame.GetInfo();

			// PreLoadCharacter(apiGame.Data.Info);

			// var apiQuest = FactoryApi.Get<ApiQuest>();
			// await apiQuest.Get();
			// await apiQuest.EmojiCheck();
			// await apiQuest.GetConfig();
			
			// var apiEvent = FactoryApi.Get<ApiEvent>();
			// await apiEvent.Get();
			// await apiEvent.Jackpot();
			// await apiEvent.GetMyJackpotHistoryList();

			var apiShop = FactoryApi.Get<ApiShop>();
			await apiShop.Get();
			// await apiShop.GetSubscriptions();

			var apiEntity = FactoryApi.Get<ApiEntity>();
			await apiEntity.Get();

			var apiChatInfo = FactoryApi.Get<ApiChatInfo>();
			await apiChatInfo.GetInfo();
			await apiChatInfo.GetConfig();

			CheckCustomProfile(apiChatInfo.Data);
			
			// CheckFirstNewAccount(apiGame.Data.Info);

			
			LoadingBar.SetProgress(0.99f);
		}

		private string GetTelegramInitData()
		{
			var telegramInitData = TelegramWebApp.InitData;
			if (!string.IsNullOrEmpty(telegramInitData)) return telegramInitData;

			var spl = Application.absoluteURL.Split('?');

#if !UNITY_EDITOR
			return spl.Length > 1 ? spl[1] : spl[0];
#endif

			return spl.Length > 1 ? spl[1] : TestTelegramUserData.Trim();
		}

		private async void PreLoadCharacter(ModelApiGameInfo gameInfo)
		{
			string charId = gameInfo.CurrentGirlId.ToString();
			await AnR.LoadAddressableByLabels<Texture>(new List<string>() { charId });
			await AnR.LoadAddressableByLabels<GameObject>(new List<string>() { charId });
		}

		private void CheckCustomProfile(ModelApiChatInfo info)
		{
			TelegramWebApp.SetLoadingProgress(1);
			if (!info.IsHaveProfile)
			{
				Signal.Send(StreamId.UI.CustomProfile);
			}
			else
			{
				SetState(State.Finished);
			}
		}

		private void SetLoadingProgress(float progress)
		{
			TelegramWebApp.SetLoadingProgress(progress);
		}

		private void SetLoadingText(string text)
		{
			TelegramWebApp.SetLoadingText(text);
		}

	}
}