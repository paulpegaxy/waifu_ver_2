using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class FriendCellViewContentTop : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private TMP_Text textTotalInvited;
		[SerializeField] private UIButton buttonLeaderboard;

		private void OnEnable()
		{
			buttonLeaderboard.onClickEvent.AddListener(OnLeaderboard);
		}

		private void OnDisable()
		{
			buttonLeaderboard.onClickEvent.RemoveListener(OnLeaderboard);
		}

		private void OnLeaderboard()
		{

		}

		public override void SetData(ModelFriendCellView model)
		{
			var data = model as ModelFriendCellViewContentTop;
			textTotalInvited.text = data.TotalInvited.ToString();
		}
	}
}
