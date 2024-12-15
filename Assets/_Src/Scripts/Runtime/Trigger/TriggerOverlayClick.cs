using System;
using Game.Core;
using Game.Model;
using Game.UI;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.OverlayClick)]
	public class TriggerOverlayClick : Trigger
	{
		private ModelTriggerOverlayClick _model;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerOverlayClick;
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			PopupTutorial.OnOverlayClick += OnOverlayClick;
			OnTrigger = callback;
		}

		public override void Unregister()
		{
			PopupTutorial.OnOverlayClick -= OnOverlayClick;
			OnTrigger = null;
		}

		private void OnOverlayClick()
		{
			OnTrigger?.Invoke(default);
		}
	}
}