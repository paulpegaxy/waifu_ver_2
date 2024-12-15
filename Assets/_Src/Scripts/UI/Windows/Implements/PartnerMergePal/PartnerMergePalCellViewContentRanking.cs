using Game.Model;
using Game.UI.Windows.Implements.LifeTimeRanking;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalCellViewContentRanking : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private LifetimeRankingCellView itemRanking;
        
        public override void SetData(AModelPartnerMergePalCellView data)
        {
            if (data is ModelPartnerMergePalCellViewContentRanking modelData)
            {
                itemRanking.SetData(modelData.Data);
            }
        }
    }
}