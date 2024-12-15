using System;
using Doozy.Runtime.UIManager;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	[Factory(ModelTriggerType.UIViewExecute)]
	public class ModelTriggerUIView : ModelTrigger
	{
		public UIId.UIViewCategory Category;
		public UIId.UIViewName Name;
		public ShowHideExecute Execute;

		public ModelTriggerUIView()
		{
			Type = ModelTriggerType.UIViewExecute;
		}
	}
}