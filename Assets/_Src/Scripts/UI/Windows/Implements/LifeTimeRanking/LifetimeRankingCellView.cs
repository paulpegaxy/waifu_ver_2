using Game.Model;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows.Implements.LifeTimeRanking
{
    public class LifetimeRankingCellView :  ESCellView<ModelLeaderboardAllTime>
    {
        [SerializeField] private ItemRanking itemRanking;
        [SerializeField] private TMP_Text textName;
        [SerializeField] private TMP_Text textScore;

        public override void SetData(ModelLeaderboardAllTime data)
        {
            textName.text = data.Name;
            
            textScore.text = data.LifeTimeScore.ToLetter();

            itemRanking.SetData(data.Rank, data.Name);
        }
    }
}