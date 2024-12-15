using System;
using Game.Runtime;
using Template.Defines;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.GameAction)]
	public class ModelTriggerGameAction : ModelTrigger
	{
		public GameAction Action;

		public ModelTriggerGameAction()
		{
			Type = ModelTriggerType.GameAction;
		}
	}
}