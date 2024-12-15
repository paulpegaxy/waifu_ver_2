using System;
using Doozy.Runtime.UIManager;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.UIPopupExecute)]
	public class ModelTriggerUIPopup : ModelTrigger
	{
		public UIId.UIPopupName Name;
		public ShowHideExecute Execute;

		public ModelTriggerUIPopup()
		{
			Type = ModelTriggerType.UIPopupExecute;
		}
	}
}