using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI
{
   public class PartnerMergePalCellViewHeaderCollab : ESCellView<AModelPartnerMergePalCellView>
   {
      [SerializeField] private ItemTimerAutoLabel itemTimer;
      [SerializeField] private Image imgBanner;
      [SerializeField] private PartnerMergePalFilterType itemMergePalFilterType;

      public static Action<TypeFilterPartner> OnClickFilter;

      private TypeFilterPartner _filterType;

      public override void SetData(AModelPartnerMergePalCellView data)
      {
         if (data is ModelPartnerMergePalCellViewHeaderCollab modelData)
         {
            _filterType = modelData.FilterType;
            itemMergePalFilterType.ActiveRankingFilter(modelData.IsHaveRankingFilter);
            itemMergePalFilterType.SetData(modelData.FilterType);

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