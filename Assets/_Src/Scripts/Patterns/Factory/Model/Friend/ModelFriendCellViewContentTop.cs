using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewContentTop : ModelFriendCellView
    {
        public float TotalInvited;

        public ModelFriendCellViewContentTop()
        {
            Type = FriendCellViewType.ContentTop;
        }
    }
}
