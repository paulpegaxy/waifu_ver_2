using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewHeaderJoined : ModelClubCellView
    {
        public ModelApiClubData Club;

        public ModelClubCellViewHeaderJoined()
        {
            Type = TypeClubCellView.HeaderJoined;
        }
    }
}
