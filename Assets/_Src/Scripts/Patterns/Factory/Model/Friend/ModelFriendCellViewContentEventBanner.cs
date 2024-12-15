using System;
using Game.Defines;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewContentEventBanner : ModelFriendCellView
    {
        public ModelApiFriendEventConfigData Config;

        public ModelFriendCellViewContentEventBanner()
        {
            Type = FriendCellViewType.ContentEventBanner;
        }
    }
}
