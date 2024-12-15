

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ItemTimerAutoLabel : MonoBehaviour
    {
        [SerializeField] protected TMP_Text txtLb;
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
            ProcessLb(_duration);
            if (_duration <= 0)
            {
                return;
            }
            
            OnManualEndTimer = manualEndCallBack;
            StartUpdateTime();
            UpdateProgress();
        }
        
        public void SetDuration(DateTime dateTimeDuration,Action manualEndCallBack=null)
        {
            // _duration = dateTimeDuration.Ticks;
            
            _duration = dateTimeDuration.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
            textTime.text = "";
            ProcessLb(_duration);
            if (_duration <= 0)
            {
                return;
            }
            
            OnManualEndTimer = manualEndCallBack;
            StartUpdateTime();
            UpdateProgress();
        }

        private void ProcessLb(float duration)
        {
            if (duration <= 0)
            {
                txtLb.text = Localization.Get(TextId.Common_EventEnded);
            }
            else
            {
                txtLb.text = Localization.Get(TextId.Common_EndIn);
            }
        }
    }
}