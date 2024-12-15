// Author: ad   -
// Created: 15/09/2024  : : 01:09
// DateUpdate: 15/09/2024

using System;
using System.Collections.Generic;
using Game.UI;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelPartnerMergePalCellViewHeaderCollab : AModelPartnerMergePalCellView
    {
        public TypeFilterPartner FilterType;
        public ModelApiEventConfig eventConfig;
        public bool IsHaveRankingFilter;
        
        public ModelPartnerMergePalCellViewHeaderCollab()
        {
            Type = TypePartnerMergePalCellView.HeaderCollab;
        }
    }
}