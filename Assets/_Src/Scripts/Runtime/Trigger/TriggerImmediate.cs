using System;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.Immediate)]
	public class TriggerImmediate : Trigger
	{
		private ModelTriggerImmediate _model;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerImmediate;
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			OnTrigger = callback;
			OnTrigger?.Invoke(default);
		}

		public override void Unregister()
		{
			OnTrigger = null;
		}
	}
}