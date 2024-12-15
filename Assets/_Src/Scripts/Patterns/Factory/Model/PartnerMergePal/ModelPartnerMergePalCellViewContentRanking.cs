using System;

namespace Game.Model
{
    [Serializable]
    public class ModelPartnerMergePalCellViewContentRanking : AModelPartnerMergePalCellView
    {
        public ModelLeaderboardAllTime Data;
        
        public ModelPartnerMergePalCellViewContentRanking()
        {
            Type = TypePartnerMergePalCellView.ContentRanking;
        }
    }
}