using Game.Extensions;
using Game.Model;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class ButtonValidateCurrencyFeature : AButtonValidateFeature
    {
        [SerializeField] private GameObject objAdd;

        protected override void ActiveFeature(bool isActive)
        {
            base.ActiveFeature(isActive);
            objAdd.SetActive(isActive);
        }

        protected override void OnValidateFeature()
        {
            if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.GameFeature))
            {
                ActiveFeature(true);
            }
            else
                ActiveFeature(false);
        }

        protected override void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
        {
            if (category == TutorialCategory.GameFeature && data.State == TutorialState.FeatureShop)
            {
                //for message feature
                ActiveFeature(true);
            }
        }
    }
}