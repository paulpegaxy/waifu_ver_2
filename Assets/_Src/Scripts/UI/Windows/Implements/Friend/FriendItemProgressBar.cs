// Author: ad   -
// Created: 22/09/2024  : : 23:09
// DateUpdate: 22/09/2024

using System;

namespace Game.UI
{
    public class FriendItemProgressBar : ItemProgressBar<DataFriendItemProgress>
    {
        
    }
    
    [Serializable]
    public class DataFriendItemProgress : DataItemProgress
    {
        public bool isClaimed;
        public TypeLeagueCharacter typeChar;
        public int value;
        
    }
}