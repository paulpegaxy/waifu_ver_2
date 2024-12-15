using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelBuddyCellViewContentUnlocked : ModelBuddyCellViewContentNormal
    {
        public ModelBuddyCellViewContentUnlocked()
        {
            Type = BuddyCellViewType.ContentUnlocked;
        }
    }
}
