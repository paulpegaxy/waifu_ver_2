using UnityEngine;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class FriendLeaderboardCellView : ESCellView<ModelFriendLeaderboard>
	{
		[SerializeField] private ItemRanking itemRanking;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text textFriendCount;
		[SerializeField] private TMP_Text textScore;

		public override void SetData(ModelFriendLeaderboard data)
		{
			textName.text = data.Name;
			textFriendCount.text = $"+{data.FriendCount} {Localization.Get(TextId.Common_Friends).ToLowerCase()}";
			textScore.text = data.Score.ToLetter();

			itemRanking.SetData(data.Rank, data.Name);
		}
	}
}