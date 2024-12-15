using System;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EventBundleCellViewHeader :  ESCellView<AModelEventBundleCellView>
    {
        [SerializeField] private Image imgBanner;
        [SerializeField] private ItemTimerAutoLabel itemTimer;
        
        public override void SetData(AModelEventBundleCellView data)
        {
            if (data is ModelEventBundleCellViewHeader modelData)
            {
                imgBanner.LoadSpriteAutoParseAsync("banner_" + modelData.EventId);
                var eventData = FactoryApi.Get<ApiEvent>().Data.EventBundleOffer;
                if (eventData.time_end != null)
                {
                    // UnityEngine.Debug.LogError("Time end: " + eventData.time_end);
                    itemTimer.SetDuration((DateTime) eventData.time_end);
                    // var timeEnd = (DateTime)eventData.time_end;
                    // var timeRemain = timeEnd.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
                    // itemTimer.SetDuration(timeRemain);
                }
            }
        }
    }
}