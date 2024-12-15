using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewHeaderJoin : ModelClubCellView
    {
        public ModelApiClubData Club;

        public ModelClubCellViewHeaderJoin()
        {
            Type = TypeClubCellView.HeaderJoin;
        }
    }
}
