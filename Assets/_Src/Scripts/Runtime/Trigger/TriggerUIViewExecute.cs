using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Core;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.UIViewExecute)]
	public class TriggerUIContainerExecute : Trigger
	{
		private ModelTriggerUIView _model;
		private SignalStream _signalStream;
		private SignalReceiver _signalReceiver;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerUIView;
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			OnTrigger = callback;

			_signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
			_signalStream = SignalStream.Get(nameof(UIContainer), nameof(UIView)).ConnectReceiver(_signalReceiver);

			if (SpecialExtensionUI.GetCurrentNode() == _model.Name.ToString())
			{
				OnTrigger?.Invoke(default);
			}
		}

		private void OnSignal(Signal signal)
		{
			UIViewSignalData data = (UIViewSignalData)signal.valueAsObject;
			if (data.execute == _model.Execute)
			{
				if (data.viewCategory == _model.Category.ToString() && data.viewName == _model.Name.ToString())
				{
					DelayTrigger().Forget();
				}
			}
		}

		private async UniTask DelayTrigger()
		{
			await UniTask.Delay(1);
			OnTrigger?.Invoke(default);
		}

		public override void Unregister()
		{
			_signalStream.DisconnectReceiver(_signalReceiver);
			_signalReceiver = null;

			OnTrigger = null;
		}
	}
}