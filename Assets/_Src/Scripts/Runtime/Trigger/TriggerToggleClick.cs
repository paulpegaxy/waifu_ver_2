using System;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.ToggleClick)]
	public class TriggerToggleClick : Trigger
	{
		private ModelTriggerToggleClick _model;
		private SignalStream _signalStream;
		private SignalReceiver _signalReceiver;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerToggleClick;

			_signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
			_signalStream = SignalStream.Get(nameof(UISelectable), nameof(UIToggle)).ConnectReceiver(_signalReceiver);
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			OnTrigger = callback;
		}

		public override void Unregister()
		{
			_signalStream.DisconnectReceiver(_signalReceiver);
			_signalReceiver = null;
			_signalStream = null;

			OnTrigger = null;
		}

		void OnSignal(Signal signal)
		{
			var data = (UIToggleSignalData)signal.valueAsObject;
			if (data.toggleCategory == _model.Category.ToString() && data.toggleName == _model.Name.ToString())
			{
				OnTrigger?.Invoke(default);
			}
		}
	}
}