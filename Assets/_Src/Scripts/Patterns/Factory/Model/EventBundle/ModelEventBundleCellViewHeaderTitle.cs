// Author: ad   -
// Created: 29/10/2024  : : 23:10
// DateUpdate: 29/10/2024

using Game.Model;

namespace Game.UI
{
    public class ModelEventBundleCellViewHeaderTitle : AModelEventBundleCellView
    {
        public ModelEventBundleCellViewHeaderTitle(string eventId) : base(eventId)
        {
            Type = TypeEventBundleCellView.HeaderTitle;
        }
    }
}