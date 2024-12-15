using System;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public abstract class TutorialStep
	{
		public static Action<TutorialCategory, ModelTutorialStep> OnEnter;
		public static Action<TutorialCategory, ModelTutorialStep> OnExit;

		protected virtual void OnInit(ModelTutorialStep stepData) { }
		protected virtual void Enter(ModelTutorialStep stepData, ModelTriggerEventData eventData) { }
		protected virtual void Exit(ModelTutorialStep stepData, ModelTriggerEventData eventData) { }

		private TutorialCategory _category;
		private Trigger _trigger;
		private ModelTutorialStep _stepData;

		public virtual void Init(TutorialCategory category, ModelTutorialStep stepData)
		{
			_category = category;
			_stepData = stepData;

			OnInit(stepData);

			_trigger = FactoryTrigger.Get<Trigger>(stepData.Enter.Type);
			_trigger.Init(stepData.Enter);
			_trigger.Register(OnEnterTrigger);
		}

		private void OnEnterTrigger(ModelTriggerEventData eventData)
		{
			_trigger.Unregister();
			_trigger = null;

			_trigger = FactoryTrigger.Get<Trigger>(_stepData.Exit.Type);
			_trigger.Init(_stepData.Exit);
			_trigger.Register(OnExitTrigger);
			
			Enter(_stepData, eventData);
			OnEnter?.Invoke(_category, _stepData);
		}

		private void OnExitTrigger(ModelTriggerEventData eventData)
		{
			_trigger.Unregister();
			_trigger = null;

			Exit(_stepData, eventData);
			OnExit?.Invoke(_category, _stepData);
		}
	}

	public class FactoryTutorial : Factory<TutorialState, TutorialStep>
	{
	}
}