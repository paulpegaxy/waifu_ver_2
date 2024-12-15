using System;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.ToggleClick, false)]
	public class ModelTriggerToggleClick : ModelTrigger
	{
		public UIId.UIToggleCategory Category;
		public UIId.UIToggleName Name;

		public ModelTriggerToggleClick()
		{
			Type = ModelTriggerType.ToggleClick;
		}
	}
}