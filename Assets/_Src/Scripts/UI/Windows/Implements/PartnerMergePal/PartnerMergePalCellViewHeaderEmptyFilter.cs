using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PartnerMergePalCellViewHeaderEmptyFilter : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private ItemTimerAutoLabel itemTimer;
        [SerializeField] private Image imgBanner;
        [SerializeField] private UIButton btnInfo;

        private void OnEnable()
        {
            btnInfo.onClickEvent.AddListener(OnInfo);    
        }

        private void OnDisable()
        {
            btnInfo.onClickEvent.RemoveListener(OnInfo);
        }


        private void OnInfo()
        {
            ControllerPopup.ShowInformation(Localization.Get(TextId.Event_GuideWaifuPrideEvent));
        }
        

        public override void SetData(AModelPartnerMergePalCellView data)
        {
            if (data is ModelPartnerMergePalHeaderEmptyFilter modelData)
            {
                itemTimer.gameObject.SetActive(modelData.eventConfig.time_end != null);
            
                if (modelData.eventConfig.time_end != null)
                {
                    // txtTime.text = modelData.eventConfig.time_end.Value.ToString("dd/MM/yyyy");
                    itemTimer.SetDuration((DateTime)modelData.eventConfig.time_end);
                }

                imgBanner.LoadSpriteAutoParseAsync("banner_event_" + modelData.eventConfig.id);
            }
        }
    }
}