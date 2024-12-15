// Author: ad   -
// Created: 17/10/2024  : : 07:10
// DateUpdate: 17/10/2024

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemTimerSlider : MonoBehaviour
    {
        [SerializeField] protected Image slider;

        protected float _duration;
        private long _totalDuration;
        
        protected CancellationTokenSource _cts;
        
        public static Action OnEndTimer;
        
        private Action OnManualEndTimer;
        

        private void OnDisable()
        {
            _cts?.Cancel();
        }

        private void StartUpdateTime()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(1), true, DelayType.DeltaTime, PlayerLoopTiming.Update, _cts.Token, UpdateProgress, null);
        }

        protected virtual void UpdateProgress(object obj = null)
        {
            _duration -= 1;
            if (_duration < 0)
            {
                _duration = 0;
                OnEndTimer?.Invoke();
                OnManualEndTimer?.Invoke();
                _cts.Cancel();
            }

            // textTime.text = ((int)_duration).ToTime();
            slider.fillAmount = Mathf.Clamp01(_duration / _totalDuration);
        }

        public void SetDuration(long duration,long totalDuration,Action manualEndCallBack=null)
        {
            _duration = duration;
            OnManualEndTimer = manualEndCallBack;
            _totalDuration = totalDuration;
            StartUpdateTime();
            UpdateProgress();
        }
        
        public void SetDuration(DateTime dateTimeDuration,long totalDuration,Action manualEndCallBack=null)
        {
            _duration = dateTimeDuration.Ticks;
            _totalDuration = totalDuration;
            OnManualEndTimer = manualEndCallBack;
            StartUpdateTime();
            UpdateProgress();
        }
    }
}