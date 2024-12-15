using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Runtime;
using Game.Model;
using Game.Defines;
using Cysharp.Threading.Tasks;

namespace Game.UI
{
	public class FriendEventWindow : UIWindow
	{
		[SerializeField] private FriendScroller scrollerEvent;
		[SerializeField] private FriendCellViewContentEvent myRank;
		[SerializeField] private RectTransform mask;

		private TypeFriendSeason _season = TypeFriendSeason.Current;

		protected override async void OnEnabled()
		{
			FriendEventSeason.OnChanged += OnSeasonChanged;

			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiFriend = FactoryApi.Get<ApiFriend>();
				await Process(_season);
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);

		}

		private void OnDisable()
		{
			FriendEventSeason.OnChanged -= OnSeasonChanged;
		}

		private async void OnSeasonChanged(TypeFriendSeason season)
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				await Process(season);
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);

			_season = season;
		}

		private async UniTask Process(TypeFriendSeason season)
		{
			var apiFriend = FactoryApi.Get<ApiFriend>();
			var config = apiFriend.Data.EventConfig;

			var seasonConfig = config.GetData(season);
			if (seasonConfig == null) return;

			var data = new List<ModelFriendCellView>
			{
				new ModelFriendCellViewHeaderEvent() {Season = season, Config = config}
			};

			var leaderboard = await apiFriend.EventLeaderboard(seasonConfig.season_number);
			for (var i = 0; i < leaderboard.list.Count; i++)
			{
				data.Add(new ModelFriendCellViewContentEvent() { Item = leaderboard.list[i] });
			}

			scrollerEvent.SetData(data);
			UpdateScrollerSize();
		}

		private void UpdateScrollerSize()
		{
			var apiFriend = FactoryApi.Get<ApiFriend>();
			var isShowMyRank = apiFriend.Data.EventLeaderboard.current_user != null;

			myRank.gameObject.SetActive(isShowMyRank);

			if (isShowMyRank)
			{
				myRank.SetData(new ModelFriendCellViewContentEvent() { Item = apiFriend.Data.EventLeaderboard.current_user, IsMyRank = true });
				mask.offsetMin = new Vector2(mask.offsetMin.x, 586f);
			}
			else
			{
				mask.offsetMin = new Vector2(mask.offsetMin.x, 360f);
			}
		}
	}
}