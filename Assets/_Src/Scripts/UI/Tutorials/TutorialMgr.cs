using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Input;
using Game.Model;
using Game.UI;
using Game.Core;
using Game.Extensions;
using Template.Defines;
using UnityEngine;

namespace Game.Runtime
{
	public class TutorialMgr : Singleton<TutorialMgr>
	{
		private readonly Dictionary<TutorialCategory, TutorialState> _tutorials = new();
		public TutorialCategory CategoryPlaying = TutorialCategory.None;
		
		private int _logTapCount;
		
		private readonly Dictionary<TutorialCategory,(int,TutorialState)> _dictUnlockConditions = new Dictionary<TutorialCategory, (int, TutorialState)>
		{
			{ TutorialCategory.Booster, ((int)TutorialCategory.Main, TutorialState.MainBooster) },
			{ TutorialCategory.Upgrade, ((int)TutorialCategory.Booster, TutorialState.Upgrade) },
			{ TutorialCategory.Undress , ((int)TutorialCategory.Upgrade, TutorialState.Undress) },
			{ TutorialCategory.GameFeature, ((int)TutorialCategory.Undress, TutorialState.FeatureRanking) },
			{ TutorialCategory.NextGirl, ((int)TutorialCategory.GameFeature, TutorialState.NextGirl) }
		};

		private void OnEnable()
		{
			// Init();
			// TutorialStep.OnEnter += OnStepEnter;
			// TutorialStep.OnExit += OnStepExit;
			// ControllerClick.OnLogTap += OnLogTap;
			// this.RegisterEvent(TypeGameEvent.GameStart, HardCheckAndSaveTutorial);
		}
		
		private void OnDisable()
		{
			// TutorialStep.OnEnter -= OnStepEnter;
			// TutorialStep.OnExit -= OnStepExit;
			// ControllerClick.OnLogTap -= OnLogTap;
			// this.RemoveEvent(TypeGameEvent.GameStart,HardCheckAndSaveTutorial);
		}

		public bool IsUnlockTutorial(TutorialCategory category)
		{
			return true;
			
			var tutStep = FactoryApi.Get<ApiGame>().Data.Info.TutorialIndex;
			var stateStep = (int)_tutorials[category];
			
			if (_dictUnlockConditions.TryGetValue(category, out var condition))
			{
				return tutStep >= condition.Item1 && stateStep >= (int)condition.Item2;
			}
			return false;
		}

		public void Init()
		{
			return;
			_tutorials.Clear();
			CategoryPlaying = TutorialCategory.None;
			var collection = FactoryCollection.Get<CollectionTutorial>().Get();
			var storage = FactoryStorage.Get<StorageTutorial>();

			int lastIndex = 0;
			foreach (var data in collection)
			{
				var model = storage.Get(data.Category);
				int index = Mathf.Min(0, (int)model.State - lastIndex);
				while (index < data.Steps.Count && data.Steps[index].PassThough)
				{
					model.State++;
					index++;
				}

				_tutorials.TryAdd(data.Category, model.State);

				lastIndex = (int)data.Steps[^1].State + 1;
			}
		}
		
		private void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
		{
			// UnityEngine.Debug.LogError("ENTER tut data: " + category + ", step : " + data.State);
		}

		private async void OnStepExit(TutorialCategory category, ModelTutorialStep data)
		{
			_tutorials[category]++;
			// UnityEngine.Debug.LogError("step exit data: " + category + ", step : " + data.State);
			if (data.SaveOnExit)
			{
				var storage = FactoryStorage.Get<StorageTutorial>();
				var model = storage.Get(category);
				model.State = _tutorials[category];
				storage.Save();
				if (CategoryPlaying == category)
					await CheckSaveTutorial(data.State);
			}

			Show(category);
		
		}

		private async UniTask CheckSaveTutorial(TutorialState step)
		{
			bool canPostSave = false;

			switch (CategoryPlaying)
			{
				case TutorialCategory.Main:
					canPostSave = step > TutorialState.MainPointCurrency;
					break;
				case TutorialCategory.Booster:
				case TutorialCategory.Upgrade:
					canPostSave = (CategoryPlaying == TutorialCategory.Booster && step >= TutorialState.MainBoosterBack) ||
					              (CategoryPlaying == TutorialCategory.Upgrade && step >= TutorialState.UpgradeActionFirstSkill);
					if (canPostSave)
					{
						BackButton.Enable();
					}
					break;
				case TutorialCategory.Undress:
					canPostSave = step >= TutorialState.Undress;
					break;
				case TutorialCategory.GameFeature:
					canPostSave = step >= TutorialState.FeatureShop;
					break;
				case TutorialCategory.NextGirl:
					canPostSave = step >= TutorialState.NextGirlConfirm;
					break;
			}

			if (canPostSave)
			{
				await FactoryApi.Get<ApiGame>().PostTutorialStep(CategoryPlaying);
			}
		}
		
		private async void HardCheckAndSaveTutorial(object data)
		{
			var apiGame = FactoryApi.Get<ApiGame>();
			var gameInfo = apiGame.Data.Info;
			var tutStepCategory = gameInfo.TutorialIndex;
			if (tutStepCategory>= (int)TutorialCategory.GameFeature)
				return;
			
			var apiUpgrade = FactoryApi.Get<ApiUpgrade>().Data;
			
			if (gameInfo.current_level_girl > 0 )
			{
				// await apiGame.PostTutorialStep(TutorialCategory.GameFeature);
				ActiveTutorial(TutorialCategory.GameFeature);
				this.PostEvent(TypeGameEvent.ReloadFeature);
				return;
			}

			if (tutStepCategory >= (int) TutorialCategory.Upgrade)
			{
				if (IsUnlockTutorial(TutorialCategory.Undress))
				{
					if (gameInfo.PointParse >= GameConsts.INIT_FIRST_UNDRESS)
					{
						// ActiveTutorial(TutorialCategory.Undress);
						this.PostEvent(TypeGameEvent.ActiveTutorialUndress,true);
					}
				}
			}
			else if (apiUpgrade.IsUnlockedFirstCard)
			{
				await apiGame.PostTutorialStep(TutorialCategory.Upgrade);
			}
			else if (gameInfo.PointPerTapParse > 1 && tutStepCategory < (int) TutorialCategory.Booster)
			{
				await apiGame.PostTutorialStep(TutorialCategory.Booster);
			}

			this.PostEvent(TypeGameEvent.ReloadFeature);
		}

		private void Show(TutorialCategory category)
		{
			var collection = FactoryCollection.Get<CollectionTutorial>().Get(category);
			var step = (int)_tutorials[category];

			var firstStep = ((int)collection.Steps[0].State);
			var lastStep = (int) collection.Steps[^1].State;
			if (step >= firstStep && step <= lastStep)
			{
				int stepIndex = step - firstStep;
				var data = collection.Steps[stepIndex];
				if (data != null)
				{
					ProcessShow(category, data);
				}
			}
		}
		
		private void ProcessShow(TutorialCategory category, ModelTutorialStep model)
		{
			var step = FactoryTutorial.Get<TutorialStep>(model.State) ?? new TutorialStepNormal();
			step.Init(category, model);
		}

		public bool IsState(TutorialCategory category, TutorialState state)
		{
			if (_tutorials.TryGetValue(category, out var tutorial))
			{
				return tutorial == state;
			}

			return false;
		}
		
		public TutorialState GetTutorialState(TutorialCategory category)
		{
			return _tutorials.GetValueOrDefault(category, TutorialState.MainFirstTimeLogin);
		}

		public bool HasOverlay(TutorialCategory category)
		{
			if (!_tutorials.ContainsKey(category)) return true;

			var collection = FactoryCollection.Get<CollectionTutorial>().Get(category);
			var state = (int)_tutorials[category];

			if (state >= collection.Steps.Count) return true;
			return collection.Steps[state].HasOverlay;
		}

		public void ActiveTutorial(TutorialCategory type)
		{
			// UnityEngine.Debug.LogError("ActiveTutorial: " + type);
			if (CategoryPlaying >= type)
				return;
			
			CategoryPlaying = type;
			switch (CategoryPlaying)
			{
				case TutorialCategory.Booster:
				case TutorialCategory.Upgrade:
					BackButton.Disable();
					break;
			}
			Show(type);
		}
	}
}