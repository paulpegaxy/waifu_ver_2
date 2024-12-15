using Game.Extensions;
using Game.Model;
using Template.Defines;

namespace Game.UI
{
    public class ButtonValidateGalleryFeature : AButtonValidateFeature
    {
        protected override void OnValidateFeature()
        {
            if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.NextGirl))
            {
                ActiveFeature(true);
            }
            else
                ActiveFeature(false);
        }

        protected override void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
        {
            if (category == TutorialCategory.NextGirl && data.State == TutorialState.NextGirl)
            {
                //for message feature
                ActiveFeature(true);
            }
        }
    }
}