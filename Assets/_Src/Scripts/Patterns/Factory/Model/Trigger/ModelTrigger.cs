using Sirenix.OdinInspector;
using Game.Core;
using Game.Runtime;

namespace Game.Model
{
	public enum ModelTriggerType
	{
		Immediate,
		ButtonClick,
		ToggleClick,
		OverlayClick,
		UIViewExecute,
		UIViewData,
		UIPopupExecute,
		GameAction,
		ApiLeaderboard
	}

	public abstract class ModelTrigger
	{
		[HideInEditorMode] public ModelTriggerType Type;
	}

	public class FactoryModelTrigger : Factory<ModelTriggerType, ModelTrigger>
	{
	}
}