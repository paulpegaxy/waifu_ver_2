using System;
using BreakInfinity;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewHeaderForClub : ModelClubCellView
    {
        public ModelClubFilter Filter;
        public ModelApiLeaderboardData My;
        public BigDouble TotalPoint;

        public ModelClubCellViewHeaderForClub()
        {
            Type = TypeClubCellView.HeaderForClub;
        }
    }
}
