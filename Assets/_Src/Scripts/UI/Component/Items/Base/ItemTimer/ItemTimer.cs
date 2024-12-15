using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using Game.Runtime;
using Unity.VisualScripting;

namespace Game.UI
{
    public class ItemTimer : MonoBehaviour
    {
        [SerializeField] protected TMP_Text textTime;

        protected float _duration;
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

            textTime.text = ((int)_duration).ToTime();
        }

        public void SetDuration(long duration,Action manualEndCallBack=null)
        {
            _duration = duration;
            textTime.text = "";
            OnManualEndTimer = manualEndCallBack;
            StartUpdateTime();
            UpdateProgress();
        }
        
        public void SetDuration(DateTime dateTimeDuration,Action manualEndCallBack=null)
        {
            // _duration = dateTimeDuration.Ticks;
            
            _duration = dateTimeDuration.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
            OnManualEndTimer = manualEndCallBack;
            StartUpdateTime();
            UpdateProgress();
        }
    }
}