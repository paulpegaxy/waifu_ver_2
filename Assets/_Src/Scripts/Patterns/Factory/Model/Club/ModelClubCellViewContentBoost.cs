using System;
using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewContentBoost : ModelClubCellView
    {
        public int Rank;
        public bool IsSelected;
        public ModelApiClubBoostInfo Info;

        public ModelClubCellViewContentBoost()
        {
            Type = TypeClubCellView.ContentBoost;
        }
    }
}
