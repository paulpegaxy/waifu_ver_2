// Author: ad   -
// Created: 15/09/2024  : : 01:09
// DateUpdate: 15/09/2024

using System;
using Game.UI;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelPartnerMergePalCellViewHeaderRanking : AModelPartnerMergePalCellView
    {
        public TypeFilterPartner FilterType;
        public ModelApiEventConfig eventConfig;
        
        public ModelPartnerMergePalCellViewHeaderRanking()
        {
            Type = TypePartnerMergePalCellView.HeaderRanking;
        }
    }
}