// Author: ad   -
// Created: 17/10/2024  : : 17:10
// DateUpdate: 17/10/2024

using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public abstract class AItemButtonGirlCardOverlayNormal : AItemButtonGirlCardOverlay
    {
        [SerializeField] protected TMP_Text txtPrice;
        [SerializeField] protected UIButton btnClick;

        protected virtual void OnEnable()
        {
            if (FactoryApi.Get<ApiGame>().Data.Info == null)
                return;
            
            btnClick.onClickEvent.AddListener(OnClick);
            TutorialStep.OnExit += OnStepExit;
            TutorialStep.OnEnter += OnStepEnter;
            ModelApiGameInfo.OnChanged += OnGameInfoChanged;
        }
        
        protected virtual void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
            TutorialStep.OnExit -= OnStepExit;
            TutorialStep.OnEnter -= OnStepEnter;
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
        }
        
        private void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            OnReloadInfo(gameInfo);
        }
        
        protected virtual void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
        {

        }

        protected virtual void OnStepExit(TutorialCategory category, ModelTutorialStep data)
        {

        }
        
        protected abstract void OnReloadInfo(ModelApiGameInfo data);
        
        protected abstract void OnClick();
    }
}