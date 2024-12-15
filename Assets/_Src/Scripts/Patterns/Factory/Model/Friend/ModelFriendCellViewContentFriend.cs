using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewContentFriend : ModelFriendCellView
    {
        public string Name;
        public int FriendCount;
        public int Score;

        public ModelFriendCellViewContentFriend()
        {
            Type = FriendCellViewType.ContentFriend;
        }
    }
}
