using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;
using BreakInfinity;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class ClubCellViewContentPersonal : ESCellView<ModelClubCellView>
	{
		[SerializeField] private ItemRanking itemRanking;
		[SerializeField] private ClubBoost clubBoost;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text textScore;
		[SerializeField] private GameObject objectYour;
		[SerializeField] private GameObject objectArrow;
		[SerializeField] private UIButton buttonView;

		private ModelApiLeaderboardData _data;

		private void OnEnable()
		{
			// buttonView.onClickEvent.AddListener(OnView);
		}

		private void OnDisable()
		{
			// buttonView.onClickEvent.RemoveListener(OnView);
		}

		private void OnView()
		{
			// UnityEngine.Debug.LogError("Vao detaIL " + _data.Id);
			// ClubWindow.OpenDetail(_data.Id);
			this.PostEvent(TypeGameEvent.ClubDetail, _data.Id);
			Signal.Send(StreamId.UI.ClubDetail);
		}

		private bool IsYourClub(ModelClubCellViewContentPersonal data)
		{
			var leaderboard = data.LeaderboardData;
			var isClub = data.Filter.FilterType == FilterType.Club;

			if (!isClub || data.UserInfo == null || data.UserInfo.Club == null) return false;
			return data.UserInfo.Club.id == leaderboard.Id;
		}

		private void SetBoostInfo(ModelClubCellViewContentPersonal data)
		{
			var leaderboard = data.LeaderboardData;
			var isShowBoost = data.Filter.FilterType == FilterType.Personal && data.Filter.FilterTimeType == FilterTimeType.Day && leaderboard.boost > 1;

			// clubBoost.gameObject.SetActive(isShowBoost);
			// if (isShowBoost)
			// {
			// 	clubBoost.SetData(leaderboard.Boost);
			// }
		}

		public override void SetData(ModelClubCellView model)
		{
			var data = model as ModelClubCellViewContentPersonal;
			var leaderboard = data.LeaderboardData;

			itemRanking.SetData(leaderboard.rankPos, leaderboard.Name);
			SetBoostInfo(data);

			textName.text = leaderboard.Name;
			textScore.text = leaderboard.PointsParse.ToLetter();

			var isClub = data.Filter.FilterType == FilterType.Club;
			objectYour.SetActive(IsYourClub(data));
			objectArrow.SetActive(false);
			buttonView.interactable = false;
			// buttonView.interactable = isClub;

			_data = leaderboard;
		}
	}
}
