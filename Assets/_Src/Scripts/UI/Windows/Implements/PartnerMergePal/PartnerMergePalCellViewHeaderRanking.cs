// Author: ad   -
// Created: 15/09/2024  : : 01:09
// DateUpdate: 15/09/2024

using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public class PartnerMergePalCellViewHeaderRanking : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private UIButton btnInfo;
        [SerializeField] private ItemTimerAutoLabel itemTimer;
         [SerializeField] private PartnerMergePalFilterType itemMergePalFilterType;
        
        public static Action<TypeFilterPartner> OnClickFilter;

        private TypeFilterPartner _filterType;

        public override void SetData(AModelPartnerMergePalCellView data)
        {
            var modelData = (ModelPartnerMergePalCellViewHeaderRanking) data;
            if (modelData != null)
            {
                _filterType = modelData.FilterType;
                itemMergePalFilterType.ActiveRankingFilter(true);
                itemMergePalFilterType.SetData(modelData.FilterType);

                if (modelData.eventConfig.time_end != null)
                {
                    // txtTime.text = modelData.eventConfig.time_end.Value.ToString("dd/MM/yyyy");
                    itemTimer.SetDuration((DateTime)modelData.eventConfig.time_end);
                }
            }
        }

        private void OnEnable()
        {
            btnInfo.onClickEvent.AddListener(OnClickInfo);    
            PartnerMergePalFilterType.OnChanged += OnChangeFilter;
        }

        private void OnDisable()
        {
            btnInfo.onClickEvent.RemoveListener(OnClickInfo);
            PartnerMergePalFilterType.OnChanged -= OnChangeFilter;
        }
        
        private void OnClickInfo()
        {
            this.ShowPopup(UIId.UIPopupName.PopupPartnerGuideRanking);
        }

        private void OnChangeFilter(TypeFilterPartner type)
        {
            if (_filterType != type)
            {
                OnClickFilter?.Invoke(type);
            }
        }
    }
}