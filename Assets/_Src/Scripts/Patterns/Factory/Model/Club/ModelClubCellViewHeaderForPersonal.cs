// Author: ad   -
// Created: 14/08/2024  : : 01:08
// DateUpdate: 14/08/2024

using System;
using BreakInfinity;
using Template.Defines;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewHeaderForPersonal : ModelClubCellView
    {
        public ModelClubFilter Filter;
        public ModelApiLeaderboardData My;
        public BigDouble TotalPoint;

        public ModelClubCellViewHeaderForPersonal()
        {
            Type = TypeClubCellView.HeaderForPersonal;
        }
    }
}
