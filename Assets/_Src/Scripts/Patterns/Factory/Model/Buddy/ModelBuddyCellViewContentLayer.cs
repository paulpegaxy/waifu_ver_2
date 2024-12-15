using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelBuddyCellViewContentLayer : ModelBuddyCellView
    {
        public ModelBuddyCellViewContentLayer()
        {
            Type = BuddyCellViewType.ContentLayer;
        }
    }
}
