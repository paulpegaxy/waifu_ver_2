using System;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class EventYukiCellViewHeader : ESCellView<AModelEventYukiCellView>
    {
        [SerializeField] private GameObject barTime;
        [SerializeField] private TMP_Text txtTime;
        [SerializeField] private TMP_Text txtDes;
        
        public override void SetData(AModelEventYukiCellView data)
        {
            if (data is ModelEventYukiCellViewHeader modelData)
            {
                // Do something
                if (modelData.eventConfig.time_end != null)
                {
                    barTime.SetActive(true);
                    txtTime.text = modelData.eventConfig.time_end.Value.ToString("dd/MM/yyyy");
                }else
                    barTime.SetActive(false);

                var apiEvent = FactoryApi.Get<ApiEvent>().Data;
                int valueReceived = apiEvent.event_data.background_yuki_claimed;
                int totalUser = 50000;
                string text = $"{valueReceived.ToFormat()}/{totalUser.ToFormat()}".SetHighlightStringGreen_5AFF0E();
                txtDes.text = string.Format(Localization.Get(TextId.Event_YukiNotiBg), text);
            }
        }
    }
}