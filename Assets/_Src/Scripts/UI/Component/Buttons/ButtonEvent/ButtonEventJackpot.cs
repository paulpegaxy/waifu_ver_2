using Game.Model;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class ButtonEventJackpot : MonoBehaviour
    {
        [SerializeField] private ItemTimer itemTimer;

        private void OnEnable()
        {
            var apiEvent = FactoryApi.Get<ApiEvent>();
            OnChanged(apiEvent.Data);

            ModelApiEvent.OnChanged += OnChanged;
        }

        private void OnDisable()
        {
            ModelApiEvent.OnChanged -= OnChanged;
        }

        private void OnChanged(ModelApiEvent model)
        {
            var jackpot = model.Jackpot;
            if (jackpot == null) return;

            var duration = jackpot.reset_at - ServiceTime.CurrentUnixTime;
            itemTimer.SetDuration(duration);
        }
    }
}