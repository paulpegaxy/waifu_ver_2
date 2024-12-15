using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelFriendCellViewContentBonus : ModelFriendCellView
    {
        public int NormalBonus;
        public int PremiumBonus;

        public ModelFriendCellViewContentBonus()
        {
            Type = FriendCellViewType.ContentBonus;
        }
    }
}
