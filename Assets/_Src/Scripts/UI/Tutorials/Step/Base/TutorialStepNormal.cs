using Game.Runtime;
using Game.Model;
using Template.Defines;

namespace Game.UI
{
	public class TutorialStepNormal : TutorialStep
	{
		protected TutorialData _tutorialData;

		protected override void OnInit(ModelTutorialStep stepData)
		{

		}

		protected override void Enter(ModelTutorialStep stepData, ModelTriggerEventData eventData)
		{
			if (stepData.HighlightObject != TutorialObject.None) _tutorialData = TutorialHelper.GetObjectByName(stepData.HighlightObject);
			if (_tutorialData != null) TutorialHelper.SetHighlight(_tutorialData, stepData.Interactable);

			ControllerPopup.ShowTutorial(stepData, _tutorialData);
		}

		protected override void Exit(ModelTutorialStep model, ModelTriggerEventData data)
		{
			if (model.HideOnExit) ControllerPopup.HideTutorial();
			if (_tutorialData != null) TutorialHelper.RemoveHighlight(_tutorialData);
		}
	}
}