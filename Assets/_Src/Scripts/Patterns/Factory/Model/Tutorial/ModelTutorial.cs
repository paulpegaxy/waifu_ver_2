using System;
using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelTutorial
    {
        public TutorialCategory Category;
        public List<ModelTutorialStep> Steps;
    }
}