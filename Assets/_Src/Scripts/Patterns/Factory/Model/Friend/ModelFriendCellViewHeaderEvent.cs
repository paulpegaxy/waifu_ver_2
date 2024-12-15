using System;
using Game.Defines;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewHeaderEvent : ModelFriendCellView
    {
        public TypeFriendSeason Season;
        public ModelApiFriendEventConfig Config;

        public ModelFriendCellViewHeaderEvent()
        {
            Type = FriendCellViewType.HeaderEvent;
        }
    }
}
