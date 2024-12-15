using UnityEngine;

namespace Game.Model
{
    public class ModelChatCellViewContentOverview : AModelChatCellView
    {
        public bool IsGirlAvatar;
        public int GirlID;
        public string GirlName;
        public string UserName;
        public string Title;
        public string Message;
        public int ReachAtLevel;
        public bool IsPremiumChar;
        
        public ModelChatCellViewContentOverview()
        {
            Type = TypeChatCellView.ContentOverview;
        }
    }
}