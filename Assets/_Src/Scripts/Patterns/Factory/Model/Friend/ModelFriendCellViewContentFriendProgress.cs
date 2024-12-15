// Author: ad   -
// Created: 22/09/2024  : : 16:09
// DateUpdate: 22/09/2024

using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    public class ModelFriendCellViewContentFriendProgress : ModelFriendCellView
    {
        public string Name;
        public int FriendId;
        public int FriendCount;
        public int Score;
        public int CurrentGirlLevel;
        public int DelaySlapTime;
        public bool IsPremiumUser;

        public List<ModelFriendConfigFriendBonus> ConfigProgress;

        public ModelFriendCellViewContentFriendProgress()
        {
            Type = FriendCellViewType.ContentFriendProgress;
        }
    }
}