using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewHeaderBoost : ModelClubCellView
    {
        public ModelClubCellViewHeaderBoost()
        {
            Type = TypeClubCellView.HeaderBoost;
        }
    }
}
