
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using UnityEngine;

public abstract class AButtonGirlLevel : AItemCheckStartGame
{
    [SerializeField] protected TMP_Text txtPrice;
    [SerializeField] protected UIButton btnClick;

    protected IServiceValidate ServiceValidate => ServiceLocator.GetService<IServiceValidate>();

    protected override void Awake()
    {
        base.Awake();
        // btnClick.gameObject.SetActive(false);
        btnClick.onLeftClickEvent.AddListener(OnClickButton);
        btnClick.gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        btnClick.onLeftClickEvent.RemoveListener(OnClickButton);
    }

    protected override void OnEnabled()
    {
        TutorialStep.OnExit += OnStepExit;
        TutorialStep.OnEnter += OnStepEnter;
        ModelApiGameInfo.OnChanged += OnGameInfoChanged;
    }

    protected override void OnDisabled()
    {
        TutorialStep.OnExit -= OnStepExit;
        TutorialStep.OnEnter -= OnStepEnter;
        ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
    }

    protected override void OnInit()
    {
        OnReloadInfo(FactoryApi.Get<ApiGame>().Data.Info);
    }

    private void OnGameInfoChanged(ModelApiGameInfo gameInfo)
    {
        OnReloadInfo(gameInfo);
    }

    protected abstract void OnClickButton();

    protected abstract void OnReloadInfo(ModelApiGameInfo data);

    protected virtual void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
    {

    }

    protected virtual void OnStepExit(TutorialCategory category, ModelTutorialStep data)
    {

    }
}