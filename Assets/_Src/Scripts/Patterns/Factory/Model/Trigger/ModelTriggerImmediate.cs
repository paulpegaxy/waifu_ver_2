using System;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.Immediate)]
	public class ModelTriggerImmediate : ModelTrigger
	{
		public ModelTriggerImmediate()
		{
			Type = ModelTriggerType.Immediate;
		}
	}
}