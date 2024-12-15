using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Core;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.UIPopupExecute)]
	public class TriggerUIPopupExecute : Trigger
	{
		private ModelTriggerUIPopup _model;
		private ModelTriggerButtonClick _modelButton;
		private SignalStream _signalStream;
		private SignalReceiver _signalReceiver;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerUIPopup;
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			OnTrigger = callback;

			_signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
			_signalStream = SignalStream.Get(nameof(UIContainer), nameof(UIPopup)).ConnectReceiver(_signalReceiver);
		}

		private void OnSignal(Signal signal)
		{
			UIPopupSignalData data = (UIPopupSignalData)signal.valueAsObject;
			if (data.execute == _model.Execute)
			{
				if (data.name == _model.Name.ToString())
				{
					DelayTrigger().Forget();
				}
			}
			// else
			// {
			// 	if (data.name.Equals(UIId.UIPopupName.PopupApiLoading.ToString()))
			// 	{
			// 		if (data.execute == ShowHideExecute.Hide)
			// 		{
			// 			DelayTrigger().Forget();
			// 		}
			// 	}
			// }
			// else
			// {
			// 	DelayTrigger().Forget();
			// }
		}

		private async UniTask DelayTrigger()
		{
			await UniTask.DelayFrame(1);
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