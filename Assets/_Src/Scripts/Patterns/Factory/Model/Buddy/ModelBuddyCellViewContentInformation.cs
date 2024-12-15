using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelBuddyCellViewContentInformation : ModelBuddyCellView
    {
        public ModelBuddyCellViewContentInformation()
        {
            Type = BuddyCellViewType.ContentInformation;
        }
    }
}
