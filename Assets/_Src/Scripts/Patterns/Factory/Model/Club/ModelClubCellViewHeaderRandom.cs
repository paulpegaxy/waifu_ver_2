using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewHeaderRandom : ModelClubCellView
    {
        public ModelClubCellViewHeaderRandom()
        {
            Type = TypeClubCellView.HeaderRandom;
        }
    }
}
