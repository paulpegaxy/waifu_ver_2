using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Sirenix.OdinInspector;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class AButtonValidateFeature : AItemCheckStartGame
    {
        [SerializeField] private GameObject objLock;
        [SerializeField] private bool isUseImage;

        [ShowIf("isUseImage")]
        [SerializeField]
        private Image imgTarget;

        private CanvasGroup _canvasGroup;

        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();


        protected override void OnEnabled()
        {
            TutorialStep.OnExit += OnStepExit;
            TutorialStep.OnEnter += OnStepEnter;
            this.RegisterEvent(TypeGameEvent.ReloadFeature, OnReloadFeature);
        }

        protected override void OnDisabled()
        {
            TutorialStep.OnExit -= OnStepExit;
            TutorialStep.OnEnter -= OnStepEnter;
            this.RemoveEvent(TypeGameEvent.ReloadFeature, OnReloadFeature);
        }

        protected override void OnInit()
        {
            ActiveFeature(true);
            return;
            
            ValidateFeature();
        }

        private void OnReloadFeature(object data)
        {
            ValidateFeature();
        }

        private void ValidateFeature()
        {
            if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.NextGirl))
                return;
            OnValidateFeature();
        }

        protected virtual void ActiveFeature(bool isActive)
        {
            if (objLock != null)
                objLock.SetActive(!isActive);
            if (TryGetComponent<UIButton>(out var btn))
            {
                btn.interactable = isActive;
            }


            if (isUseImage && imgTarget != null)
            {
                imgTarget.material = isActive ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
            }
            else
            {

                CanvasGroup.alpha = isActive ? 1f : 0.2f;
                CanvasGroup.blocksRaycasts = isActive;
            }
        }


        protected abstract void OnValidateFeature();

        protected abstract void OnStepEnter(TutorialCategory category, ModelTutorialStep data);

        protected virtual void OnStepExit(TutorialCategory category, ModelTutorialStep data)
        {
        }
    }
}