using System;
using Game.Defines;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewContentEvent : ModelFriendCellView
    {
        public ModelApiFriendEventLeaderboardData Item;
        public bool IsMyRank;

        public ModelFriendCellViewContentEvent()
        {
            Type = FriendCellViewType.ContentEvent;
        }
    }
}
