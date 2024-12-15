using System;
using Sirenix.OdinInspector;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelTutorialStep
    {
        public TutorialState State;
        public TutorialAlignment Alignment;
        public TextId TextId;
        public TutorialObject HighlightObject;
        [ShowInInspector] public ModelTrigger Enter;
        [ShowInInspector] public ModelTrigger Exit;
        [EnumToggleButtons] public TutorialStepConfig Config = TutorialStepConfig.HasOverlay | TutorialStepConfig.HideOnExit | TutorialStepConfig.Interactable;

        public bool HasOverlay => Config.HasFlag(TutorialStepConfig.HasOverlay);
        public bool HideOnExit => Config.HasFlag(TutorialStepConfig.HideOnExit);
        public bool Interactable => Config.HasFlag(TutorialStepConfig.Interactable);
        public bool PassThough => Config.HasFlag(TutorialStepConfig.PassThough);
        public bool SaveOnExit => Config.HasFlag(TutorialStepConfig.SaveOnExit);
    }

    [Flags]
    public enum TutorialStepConfig
    {
        HasOverlay = 1 << 1,
        HideOnExit = 1 << 2,
        Interactable = 1 << 3,
        PassThough = 1 << 4,
        SaveOnExit = 1 << 5,
    }
}