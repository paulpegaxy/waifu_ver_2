// Author: ad   -
// Created: 15/09/2024  : : 01:09
// DateUpdate: 15/09/2024

using System;
using Game.UI;

namespace Game.Model
{
    [Serializable]
    public abstract class AModelPartnerMergePalCellView : IESModel<TypePartnerMergePalCellView>
    {
        public MainWindowAction TypePartner { get; set; }
        public TypePartnerMergePalCellView Type { get; set; }
    }
}