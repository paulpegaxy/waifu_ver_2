using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

public class GirlEntity : Entity
{
    public static Action<int> OnLevelUp;
    
    public GirlSpineAnimation Animation { get;private set; }
    
    public GirlStateMachine StateMachine { get; private set; }
    
    public TypeGirlReact TypeReact { get; private set; }
    
    private CancellationTokenSource _ctsDetectClick;

    private float _lastTimeClick;
    
    private void OnDestroy()
    {
        ControllerClick.OnClick -= HandleGirlClick;
        ModelApiEntity.OnChanged -= LevelUpGirl;
        _ctsDetectClick?.Cancel();
    }


    private void StartUpdateTime()
    {
        _ctsDetectClick?.Cancel();
        _ctsDetectClick = new CancellationTokenSource();
    }

    public override async void Init(int id,Action onComplete=null)
    {
        TypeReact = TypeGirlReact.None;
        await SpawnAnimContainer(id);

        SetStateMachine(id);
        RegisterEvent();
        // OnChangeVisual(FactoryApi.Get<ApiGame>().Data.Info.current_level_girl);
        OnChangeVisual(SpecialExtensionGame.GetLevelWaifu(id));
        
        onComplete?.Invoke();
    }

    public override async void InitToShowReward(int id,Action onComplete=null)
    {
        await SpawnAnimContainer(id);
        SetStateMachine(id);
        OnChangeVisual(0);
        onComplete?.Invoke();
    }

    private void RegisterEvent()
    {
        ControllerClick.OnClick += HandleGirlClick;
        ModelApiEntity.OnChanged += LevelUpGirl;
    }

    public override void DestroyObject()
    {
        ControllerSpawner.Instance.Return(AnimContainer);
    }

    private void SetStateMachine(int id)
    {
        StateMachine = new GirlStateMachine(this);
        StateMachine.SetState(GirlStateType.Idle);
        SetData(id);
    }
    
    private async UniTask SpawnAnimContainer(int id)
    {
        if (AnimContainer != null)
        {
            ControllerSpawner.Instance.Return(AnimContainer);
            AnimContainer = null;
        }
        AnimContainer = await ControllerSpawner.Instance.SpawnAsync($"{id}_spine");
        AnimContainer.transform.SetParent(transform);
        AnimContainer.SetActive(true);
        AnimContainer.transform.localPosition = Vector3.zero;
        AnimContainer.transform.localScale = Vector3.one;
        if (AnimContainer != null)
            Animation = new GirlSpineAnimation(AnimContainer);
    }

    public override void Reset()
    {
        StateMachine.Exit();

    }

    private void HandleGirlClick(TypeGirlReact type)
    {
        TypeReact = type;
        _lastTimeClick = Time.time;
        if (_ctsDetectClick == null)
        {
            StartUpdateTime();
        }
        StateMachine.SetState(GirlStateType.React);
    }

    private void LevelUpGirl(ModelApiEntity model)
    {
        if (model==null) return;
        
        StateMachine.SetState(GirlStateType.LevelUp);
    }

    public void OnChangeVisual(int currLevel)
    {
        Animation.ChangeVisual(currLevel + 1);
    }

    public void Undress(int levelUndress)
    {
        Animation.Undress(levelUndress + 1);
    }
    
    public async UniTask DoneUndress(float duration)
    {
        await Animation.DoneUndress(duration);
    }
}
