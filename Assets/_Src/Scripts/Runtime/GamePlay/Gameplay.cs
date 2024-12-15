using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Game.Extensions;
using Game.Model;
using Game.UI;
using Template.Defines;

namespace Game.Runtime
{
    public enum TypeTapGirlTutorial
    {
        None,
        FirstTapGirl,
        BoosterTapGirl,
        UndressTapGirl,
        UpgradeTapGirl,
    }

    public class Gameplay : GameplayListener
    {
        private ApiGame _apiGame;
        // private bool _isPrediction = false;
        private int _tapCount = 0;
        private int _tapCountForTut = 0;

        private CancellationTokenSource _ctsDetectClick;

        private float _lastTimeClick;
        private bool _isClick = false;
        // private int _tapActiveTut;
        private bool _isInterruptGame = false;

        // private TypeTapGirlTutorial _typeTapGirlTutorial = TypeTapGirlTutorial.None;

        protected override void OnEnable()
        {
            base.OnEnable();
            _isClick = false;
            _isInterruptGame = false;
            _tapCount = 0;

            TutorialStep.OnExit += OnTutorialStepExit;
            ControllerClick.OnLogTap += OnLogTap;
            this.RegisterEvent(TypeGameEvent.GameStart, OnGameStart);
            this.RegisterEvent(TypeGameEvent.InterruptGame, OnInterruptGame);
            _apiGame = FactoryApi.Get<ApiGame>();
            GameplayInterrupt.OnVisibilityChanged += OnInterrupt;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            TutorialStep.OnExit -= OnTutorialStepExit;
            ControllerClick.OnLogTap -= OnLogTap;
            _ctsDetectClick?.Cancel();

            this.RemoveEvent(TypeGameEvent.GameStart, OnGameStart);
            this.RemoveEvent(TypeGameEvent.InterruptGame, OnInterruptGame);
            GameplayInterrupt.OnVisibilityChanged -= OnInterrupt;
        }

        private void Update()
        {
            ControllerRefreshTimer.Update(Time.deltaTime);
        }


        private void OnInterruptGame(object data)
        {
            _isInterruptGame = true;
        }

        private void OnInterrupt(bool isVisible)
        {
            PostLogTapTutorial();
        }

        private void OnApplicationQuit()
        {
            AnR.ReleaseAll();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            PostLogTapTutorial().Forget();
        }

        protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            if (!_isInterruptGame)
                ControllerRefreshTimer.SetDuration(RefreshTimerType.GameInfo, GameConsts.GAME_INFO_REFRESH_TIME);
        }

        private void OnGameStart(object data)
        {
            CheckAudio();
            var apiGame = FactoryApi.Get<ApiGame>();
            apiGame.Data.Info.Notification();

            var apiEvent = FactoryApi.Get<ApiEvent>();
            apiEvent.Data.Notification();

            CheckStorageSession();

            var isFresher = apiGame.Data.Info.IsFresher();

            if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.GameFeature) || !isFresher)
            {
                Setup();
            }
            else
            {
                if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.Main))
                    TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Main);
            }

            GameUtils.Log("red", "-------------GAME START-------------");
        }

        private void CheckStorageSession()
        {
            ServiceLocator.GetService<IServiceTracking>().SendTrackingSessionLength();
        }

        private void CheckAudio()
        {
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            var model = storageSetting.Get();

            if (!model.isSound)
                ControllerAudio.Instance.Mute(AudioMixerType.Sfx);
            else
                ControllerAudio.Instance.Unmute(AudioMixerType.Sfx);

            if (model.isMusic)
            {
                ControllerAudio.Instance.Unmute(AudioMixerType.Bgm);
                ControllerAudio.Instance.PlayBgm(AnR.AudioKey.Bgm);
            }
            else
                ControllerAudio.Instance.Mute(AudioMixerType.Bgm);
        }

        private void StartUpdateTime()
        {
            _isClick = true;
            _ctsDetectClick?.Cancel();
            _ctsDetectClick = new CancellationTokenSource();
            PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(1),
                true, DelayType.DeltaTime, PlayerLoopTiming.Update, _ctsDetectClick.Token, UpdateProgress, null);
        }

        private void UpdateProgress(object obj)
        {
            if (Time.time - _lastTimeClick > 2)
            {
                // ServiceLocator.GetService<IServiceSyncData>().SyncForTapCount(_tapCount);
                // _tapCount = 0;

                StopClick();
            }
        }

        private void OnTutorialStepExit(TutorialCategory category, ModelTutorialStep step)
        {
            switch (category)
            {
                // case TutorialCategory.Main:
                //     HandleMainCategory(step);
                //     break;
                // case TutorialCategory.Booster:
                //     if (step.State == TutorialState.MainBoosterGuideTap)
                //     {
                //         _typeTapGirlTutorial = TypeTapGirlTutorial.UpgradeTapGirl;
                //         _tapActiveTut = GameConsts.MAX_LOG_TAP_FOR_FIRST_TIME;
                //     }
                //     break;
                // case TutorialCategory.Upgrade:
                //     if (step.State == TutorialState.UpgradeGuideProfit)
                //     {
                //         PostLogTapTutorial();
                //         _typeTapGirlTutorial = TypeTapGirlTutorial.UndressTapGirl;
                //         _tapActiveTut = GameConsts.MAX_LOG_TAP_FOR_GAME_FEATURE_TUT;
                //     }
                //     break;
                case TutorialCategory.GameFeature:
                    if (step.State == TutorialState.FeatureRankingBack)
                    {
                        Setup();
                    }
                    break;
            }
        }

        // private void HandleMainCategory(ModelTutorialStep step)
        // {
        //     if (step.State == TutorialState.MainFirstTimeLogin)
        //     {
        //         _typeTapGirlTutorial = TypeTapGirlTutorial.FirstTapGirl;
        //         _tapActiveTut = GameConsts.MAX_LOG_TAP_FOR_FIRST_TIME;
        //     }
        //     else if (step.State == TutorialState.MainPointCurrency)
        //     {
        //         _typeTapGirlTutorial = TypeTapGirlTutorial.BoosterTapGirl;
        //         _tapActiveTut = GameConsts.MAX_LOG_TAP_FOR_BOOSTER_TUT;
        //     }
        // }

        private void StopClick()
        {
            _isClick = false;
            _ctsDetectClick?.Cancel();
            _ctsDetectClick = null;
        }

        private void OnLogTap()
        {
            if (ControllerResource.Get(TypeResource.ExpWaifu).Amount >= FactoryApi.Get<ApiGame>().Data.Info.PointPerTapParse)
            {
                _tapCount++;
                _lastTimeClick = Time.time;
                if (!_isClick)
                {
                    StartUpdateTime();
                }


                ServiceLocator.GetService<IServiceGirlSfx>().CheckGirlSfx(_tapCount);
            }
            else
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughEnergy));
                int temp = _tapCount;
                _tapCount = 0;
                // ServiceLocator.GetService<IServiceSyncData>().SyncForTapCount(temp);
                StopClick();
            }

            CheckLogTapForTutorial();
        }

        private async UniTask PostLogTapTutorial()
        {
            if (_tapCountForTut > 0)
            {
                await ServiceLocator.GetService<IServiceSyncData>().SyncForTapCount(_tapCountForTut);
                _tapCountForTut = 0;
            }
        }

        private void CheckLogTapForTutorial()
        {
            // if (_tapCount >= _tapActiveTut && _typeTapGirlTutorial != TypeTapGirlTutorial.None)
            // {
            //     ProcessEnterTutorial();
            // }
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;

            if (gameInfo.TutorialIndex >= (int)TutorialCategory.GameFeature)
                return;

            var tutCategory = (TutorialCategory)Mathf.Max((int)TutorialCategory.Main, gameInfo.TutorialIndex);
            var currStep = TutorialMgr.Instance.GetTutorialState(tutCategory);

            // Use a dictionary to map categories to their respective handlers
            var categoryHandlers = new Dictionary<TutorialCategory, Action<TutorialState>>
            {
                { TutorialCategory.Main, HandleAtMainCategory },
                { TutorialCategory.Booster, HandleUpgradeCategory },
                { TutorialCategory.Upgrade, HandleUndressCategory },
                { TutorialCategory.Undress, HandleGameFeatureCategory }
            };

            if (categoryHandlers.TryGetValue(tutCategory, out var handler))
            {
                // UnityEngine.Debug.LogError("curr step: " + currStep + ", tut category: " + tutCategory +
                //                            ", tut Index: " + gameInfo.TutorialIndex);
                handler(currStep);
            }
        }

        private void Setup()
        {
            // UnityEngine.Debug.LogError("STEEEEEEEEEEEEEEEEEEUP");
            ControllerRefreshTimer.Add(RefreshTimerType.RefreshToken, () => _apiGame.RefreshToken().Forget(), GameConsts.GAME_INFO_REFRESH_TOKEN, true);
            ControllerRefreshTimer.Add(RefreshTimerType.GameInfo, ProcessSync, GameConsts.GAME_INFO_REFRESH_TIME, true);
            ControllerRefreshTimer.Add(RefreshTimerType.ServerTime, () => ServiceTime.Refresh().Forget(), GameConsts.SERVER_TIME_REFRESH_TIME, true);
        }

        private void ProcessSync()
        {
            if (_isInterruptGame)
                return;

            if (SpecialExtensionGame.IsMaintainance())
            {
                StopClick();
                ControllerAutomation.Stop();
                TelegramWebApp.Reload();
                return;
            }

            var serviceSyncData = ServiceLocator.GetService<IServiceSyncData>();

            if (_isClick || _tapCount > 0 || ControllerAutomation.IsStarted)
            {
                // UnityEngine.Debug.LogError("Vao syn manual");
                int tempTapCount = _tapCount;
                // UnityEngine.Debug.LogError("tap init: " + tempTapCount);
                // if (tempTapCount >= GameConsts.MAX_TAP_COUNT_TO_SEND)
                // {
                _ = serviceSyncData.SyncForTapCount(tempTapCount, () =>
                {
                    _tapCount -= tempTapCount;
                    if (_tapCount < 0)
                        _tapCount = 0;
                });
                // }

                return;
            }

            FactoryApi.Get<ApiGame>().GetInfo().Forget();
        }

        private void HandleAtMainCategory(TutorialState currStep)
        {
            if (currStep == TutorialState.MainFirstTimeLogin && _tapCount >= 1)
            {
                _tapCount = 0;
                // this.PostEvent(TypeGameEvent.FristTapGirlTut);
            }
            else if (currStep == TutorialState.MainFirstWaitTapGirl && _tapCount >= GameConsts.MAX_LOG_TAP_FOR_FIRST_TIME)
            {
                //3 = 1993
                // UnityEngine.Debug.LogError("Vao day: "+_tapCount+", tap count for tut: "+_tapCountForTut);
                _tapCountForTut += _tapCount;
                _tapCount = 0;
                this.PostEvent(TypeGameEvent.FristTapGirlTut);
            }
            else if (currStep >= TutorialState.MainPointCurrency && _tapCount >= GameConsts.MAX_LOG_TAP_FOR_BOOSTER_TUT)
            {
                // UnityEngine.Debug.LogError("Tap count: " + _tapCount+", tap count for tut: "+_tapCountForTut);
                //5 = 1998
                _tapCountForTut += _tapCount;
                _tapCount = 0;
                PostLogTapTutorial().Forget();
                TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Booster);
            }
        }

        private async void HandleUpgradeCategory(TutorialState currStep)
        {
            // UnityEngine.Debug.LogError("Curr    : " + currStep);
            if (currStep >= TutorialState.MainBoosterActionUpgrade && _tapCount >= GameConsts.MAX_LOG_TAP_FOR_UPGRADE_TUT)
            {
                //1 = 998 + 2= 1000
                _tapCountForTut += _tapCount;
                _tapCount = 0;
                await PostLogTapTutorial();
                TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Upgrade);
            }
        }

        private async void HandleUndressCategory(TutorialState currStep)
        {
            if (currStep >= TutorialState.UpgradeActionFirstSkill && _tapCount >= GameConsts.MAX_LOG_TAP_FOR_GAME_FEATURE_TUT)
            {
                //10 = 20
                _tapCountForTut += _tapCount;
                _tapCount = 0;
                await PostLogTapTutorial();
                this.PostEvent(TypeGameEvent.ActiveTutorialUndress, true);

                // TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Undress);
            }
        }

        private void HandleGameFeatureCategory(TutorialState currStep)
        {
            if (currStep == TutorialState.UndressDone && _tapCount >= GameConsts.MAX_LOG_TAP_FOR_GAME_FEATURE_TUT)
            {
                //10 = 20
                _tapCountForTut += _tapCount;
                _tapCount = 0;
                PostLogTapTutorial().Forget();
                TutorialMgr.Instance.ActiveTutorial(TutorialCategory.GameFeature);
            }
        }
    }
}