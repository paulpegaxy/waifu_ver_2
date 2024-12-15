
using Game.UI;
using Template.Defines;

namespace Game.Model
{
    public abstract class ModelFriendCellView : IESModel<FriendCellViewType>
    {
        public FriendCellViewType Type { get; set; }
    }
}
