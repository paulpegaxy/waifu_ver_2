using System;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Core;
using Game.Model;
using Doozy.Runtime.UIManager.Input;
using Cysharp.Threading.Tasks;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.ButtonClick)]
	public class TriggerButtonClick : Trigger
	{
		private ModelTriggerButtonClick _model;
		private SignalStream _signalStream;
		private SignalReceiver _signalReceiver;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerButtonClick;

			_signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
			_signalStream = SignalStream.Get(nameof(UISelectable), nameof(UIButton)).ConnectReceiver(_signalReceiver);
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

		async void OnSignal(Signal signal)
		{
			var data = (UIButtonSignalData)signal.valueAsObject;
			if (data.buttonCategory == _model.Category.ToString() && data.buttonName == _model.Name.ToString())
			{
				OnTrigger?.Invoke(default);
				if (data.buttonCategory == UIId.UIButtonCategory.Navigation.ToString() && data.buttonName == UIId.UIButtonName.Back.ToString())
				{
					await UniTask.Delay(250);
					BackButton.Fire();
				}
			}
		}
	}
}