using System;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.UIViewData)]
	public class ModelTriggerUIViewData : ModelTrigger
	{
		public UIId.UIViewCategory Category;
		public UIId.UIViewName Name;

		public ModelTriggerUIViewData()
		{
			Type = ModelTriggerType.UIViewData;
		}
	}
}