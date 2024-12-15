using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

public abstract class AItemResourceTimer : MonoBehaviour, IItemResourceTimer
{
    private CancellationTokenSource _cts;
    
    protected virtual void OnEnable()
    {
        StartUpdateTime();
    }
    
    protected void OnDisable()
    {
        _cts?.Cancel();
    }

    private void StartUpdateTime()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(1), true, DelayType.DeltaTime, PlayerLoopTiming.Update, _cts.Token, UpdateProgress, null);
    }


    protected abstract void UpdateProgress(object obj);
}