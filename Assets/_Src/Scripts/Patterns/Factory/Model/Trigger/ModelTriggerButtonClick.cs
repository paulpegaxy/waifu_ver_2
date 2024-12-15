using System;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.ButtonClick, false)]
	public class ModelTriggerButtonClick : ModelTrigger
	{
		public UIId.UIButtonCategory Category;
		public UIId.UIButtonName Name;

		public ModelTriggerButtonClick()
		{
			Type = ModelTriggerType.ButtonClick;
		}
	}
}