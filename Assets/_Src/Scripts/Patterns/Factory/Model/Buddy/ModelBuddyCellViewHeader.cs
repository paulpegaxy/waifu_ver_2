using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelBuddyCellViewHeader : ModelBuddyCellView
    {
        public float Processing;
        public float Total;

        public ModelBuddyCellViewHeader()
        {
            Type = BuddyCellViewType.Header;
        }
    }
}
