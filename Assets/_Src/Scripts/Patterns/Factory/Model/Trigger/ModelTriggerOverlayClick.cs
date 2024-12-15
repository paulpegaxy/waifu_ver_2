using System;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.OverlayClick)]
	public class ModelTriggerOverlayClick : ModelTrigger
	{
		public ModelTriggerOverlayClick()
		{
			Type = ModelTriggerType.OverlayClick;
		}
	}
}