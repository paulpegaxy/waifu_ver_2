using System;
using Game.Core;
using Game.Model;
using Game.UI;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.UIViewData)]
	public class TriggerUIViewData : Trigger
	{
		private ModelTriggerUIViewData _model;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerUIViewData;
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			// UnityEngine.Debug.Log("VAO REGISTER: " + _model.Name);
			UIWindow.OnDataLoaded += OnDataLoaded;
			OnTrigger = callback;
		}

		public override void Unregister()
		{
			UIWindow.OnDataLoaded -= OnDataLoaded;
			OnTrigger = null;
		}

		private void OnDataLoaded(UIId.UIViewCategory category, UIId.UIViewName name)
		{
			if (category == _model.Category && name == _model.Name)
			{
				OnTrigger?.Invoke(default);
			}
		}
	}
}