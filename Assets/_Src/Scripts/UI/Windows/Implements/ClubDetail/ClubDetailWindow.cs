using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Input;
using Game.Core;
using Game.Extensions;
using Game.Runtime;
using Game.Model;
using Template.Defines;

namespace Game.UI
{
	public class ClubDetailWindow : UIWindow
	{
		[SerializeField] private ClubScroller _scrollerMain;

		private FilterTimeType _filterTime = FilterTimeType.Lifetime;
		private ModelApiClubData _data;
		private int _clubId = -1;
		private string _previousNode;

		protected override void OnEnabled()
		{
			_previousNode = SpecialExtensionUI.GetPreviousNode();

			ClubFilterTime.OnChanged += OnFilterTimeChanged;
			ClubLeagueRibbon.OnView += OnClub;

			ClubCellViewHeaderJoin.OnBoostClub += OnBoostClub;
			ClubCellViewHeaderJoin.OnJoinClub += OnJoinClub;
			ClubCellViewHeaderJoined.OnBoostClub += OnBoostClub;
			ClubCellViewHeaderJoined.OnLeaveClub += OnLeaveClub;

			this.ShowProcessing();
			try
			{
				Fetch();
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		protected override void OnDisabled()
		{
			var currentNode = SpecialExtensionUI.GetCurrentNode();
			if (currentNode == UIId.UIViewName.ClubRandom.ToString() || currentNode == UIId.UIViewName.Main.ToString()) _clubId = -1;

			ClubFilterTime.OnChanged -= OnFilterTimeChanged;
			ClubLeagueRibbon.OnView -= OnClub;

			ClubCellViewHeaderJoin.OnBoostClub -= OnBoostClub;
			ClubCellViewHeaderJoin.OnJoinClub -= OnJoinClub;
			ClubCellViewHeaderJoined.OnBoostClub -= OnBoostClub;
			ClubCellViewHeaderJoined.OnLeaveClub -= OnLeaveClub;
		}

		private void OnFilterTimeChanged(FilterTimeType filter)
		{
			_filterTime = filter;
			Refresh();
		}

		private void OnClub(ModelApiClubData data)
		{
			this.PostEvent(TypeGameEvent.Club, new ModelClubFilter()
			{
				FilterType = FilterType.Club,
				FilterTimeType = FilterTimeType.Day,
				typeLeagueIndex = (int) data.league,
			});

			if (_previousNode == UIId.UIViewName.Club.ToString())
			{
				BackButton.Fire();
			}
			else
			{
				Signal.Send(StreamId.UI.Club);
			}
		}

		private async void OnBoostClub(ModelApiClubData data)
		{
			var apiClub = FactoryApi.Get<ApiClub>();
			await apiClub.Get();

			var is1st = apiClub.Data.GetRank(data.id) == 1;
			if (is1st)
			{
				ControllerPopup.ShowToast(Localization.Get(TextId.Toast_ClubBoostMax));
			}
			else
			{
				this.PostEvent(TypeGameEvent.ClubBoost, data);
				Signal.Send(StreamId.UI.ClubBoost);
			}
		}

		private async void OnJoinClub(ModelApiClubData data)
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiClub = FactoryApi.Get<ApiClub>();
				await apiClub.Join(data.id);

				Fetch();
				ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Toast_ClubJoined), data.name));
				// ControllerPopup.ShowInformation(string.Format(Localization.Get(TextId.Toast_ClubJoined),
				// 	data.name, FactoryApi.Get<ApiUser>().Data.User.name));
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);
		}

		private async void OnLeaveClub(ModelApiClubData data)
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiClub = FactoryApi.Get<ApiClub>();
				await apiClub.Leave(data.id);

				Fetch();
				ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Toast_ClubLeft), data.name));
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);
		}

		private async void Fetch()
		{
			var clubId = this.GetEventData<TypeGameEvent,int>(TypeGameEvent.ClubDetail);
			var apiClub = FactoryApi.Get<ApiClub>();

			if (_previousNode == UIId.UIViewName.ClubRandom.ToString() || _previousNode == UIId.UIViewName.Main.ToString())
			{
				if (_clubId == -1)
				{
					_clubId = clubId;
				}
				_data = await apiClub.GetClub(_clubId);
			}
			else
			{
				_data = await apiClub.GetClub(clubId);
			}

			Refresh();
		}

		private void Refresh()
		{
			var apiUser = FactoryApi.Get<ApiUser>();
			var userInfo = apiUser.Data;

			var data = new List<ModelClubCellView>();
			if (userInfo.Club != null && userInfo.Club.id == _data.id)
			{
				data.Add(new ModelClubCellViewHeaderJoined() { Club = _data });
			}
			else
			{
				data.Add(new ModelClubCellViewHeaderJoin() { Club = _data });
			}

			// var leaderboard = _filterTime == FilterTimeType.Day ? _data.leaderboard_daily : _data.leaderboard_weekly;
			
			foreach (var item in _data.leaderboard_all_time)
			{
				data.Add(new ModelClubCellViewContentPersonal()
				{
					Filter = new ModelClubFilter()
					{
						FilterType = FilterType.Club,
						FilterTimeType = _filterTime,
						typeLeagueIndex = (int) _data.league
					},
					LeaderboardData = item
				});
			}

			_scrollerMain.SetData(data);
		}
	}
}