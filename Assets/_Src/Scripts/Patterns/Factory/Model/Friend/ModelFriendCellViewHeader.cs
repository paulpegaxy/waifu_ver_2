using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewHeader : ModelFriendCellView
    {
        public string Title;

        public ModelFriendCellViewHeader()
        {
            Type = FriendCellViewType.Header;
        }
    }
}
