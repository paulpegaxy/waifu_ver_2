using System.Collections;
using System.Collections.Generic;
using Game.Extensions;
using Game.Model;
using Game.UI;
using Template.Defines;
using UnityEngine;

public class ButtonValidateMessageFeature : AButtonValidateFeature
{
    protected override void OnValidateFeature()
    {
        if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.Undress))
        {
            ActiveFeature(true);
        }
        else
            ActiveFeature(false);
    }

    protected override void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
    {
        if (category == TutorialCategory.Undress && data.State == TutorialState.Undress)
        {
            //for message feature
            ActiveFeature(true);
        }
    }
}
