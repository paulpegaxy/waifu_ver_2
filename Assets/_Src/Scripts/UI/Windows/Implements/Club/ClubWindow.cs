using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Core;
using Game.Extensions;
using Game.Runtime;
using Game.Model;
using Template.Defines;
using Unity.VisualScripting;

namespace Game.UI
{
	public class ClubWindow : UIWindow
	{
		[SerializeField] private ClubScroller clubScroller;
		[SerializeField] private ClubCellViewContentPersonal myRank;
		[SerializeField] private UIButton buttonInfo;

		private ModelClubFilter _filter;
		private List<ModelClubCellView> _listData = new List<ModelClubCellView>();

		protected override void OnEnabled()
		{
			var filter = this.GetEventData<TypeGameEvent,ModelClubFilter>(TypeGameEvent.Club, true);
			if (filter != null)
			{
				_filter = filter;
			}

			Refresh();

			buttonInfo.onClickEvent.AddListener(OnInfo);
			ClubLeague.OnChanged += OnLeaderboardLeagueChanged;
			ClubLeagueGirl.OnChanged += OnLeaderboardPersonalChanged;
			ClubFilterType.OnChanged += OnLeaderboardTypeFilter;
			ClubFilterTime.OnChanged += OnLeaderboardTimeFilter;
		}

		protected override void OnDisabled()
		{
			buttonInfo.onClickEvent.RemoveListener(OnInfo);
			ClubLeague.OnChanged -= OnLeaderboardLeagueChanged;
			ClubLeagueGirl.OnChanged -= OnLeaderboardPersonalChanged;
			ClubFilterType.OnChanged -= OnLeaderboardTypeFilter;
			ClubFilterTime.OnChanged -= OnLeaderboardTimeFilter;
		}

		private void OnLeaderboardLeagueChanged(TypeLeague leagueType)
		{
			_filter.typeLeagueIndex =(int) leagueType;
			Refresh();
		}

		private void OnLeaderboardPersonalChanged(int typeIndex)
		{
			_filter.typeLeagueIndex = typeIndex;
			// UnityEngine.Debug.LogError("Type Changed girl " + typeIndex);
			Refresh();
		}

		private void OnLeaderboardTypeFilter(FilterType filterType)
		{
			_filter.FilterType = filterType;
			Refresh();
		}

		private void OnLeaderboardTimeFilter(FilterTimeType filterTimeType)
		{
			_filter.FilterTimeType = filterTimeType;
			Refresh();
		}

		private async void Refresh()
		{
			this.ShowProcessing();
			try
			{
				//TODO: only process life time filter
				_filter.FilterTimeType = FilterTimeType.Lifetime;
				_listData = new List<ModelClubCellView>();
				myRank.gameObject.SetActive(false);
				var apiLeaderboard = FactoryApi.Get<ApiLeaderboard>();
				
				if (_filter.FilterType == FilterType.Personal)
				{
					var leaderboardRank = await apiLeaderboard.Get(_filter);
				
					if (leaderboardRank.current_user_leaderboard != null)
					{
						leaderboardRank.current_user_leaderboard.rank = leaderboardRank.global_rank.ToString();
					}
					LoadPersonalRanking(leaderboardRank);

				}
				else
				{
					await LoadClubRanking();
					// myRank.gameObject.SetActive(true);
				}

				clubScroller.SetData(_listData);
				OnDataLoaded?.Invoke(UIId.UIViewCategory.Window, UIId.UIViewName.Club);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private void LoadPersonalRanking(ModelApiLeaderboardRank leaderboardRank)
		{
			var apiUpgrade = FactoryApi.Get<ApiUpgrade>();
			_listData.Add(new ModelClubCellViewHeaderForPersonal()
			{
				Filter = _filter,
				My = leaderboardRank.current_user_leaderboard,
				TotalPoint = apiUpgrade.Data.PointAllTimeParse
			});
			
			foreach (var item in leaderboardRank.leaderboard)
			{
				_listData.Add(new ModelClubCellViewContentPersonal()
				{
					Filter = _filter,
					LeaderboardData = item,
					UserInfo = GetUserInfo()
				});
			}
		}

		private async UniTask LoadClubRanking()
		{
			var clubList = await FactoryApi.Get<ApiLeaderboard>().GetClubs();
			
			
			_listData.Add(new ModelClubCellViewHeaderForClub()
			{
				Filter = _filter,
				// My = leaderboardRank.current_user_leaderboard,
				// TotalPoint = apiUpgrade.Data.PointAllTimeParse
			});
			
			
			foreach (var item in clubList)
			{
				_listData.Add(new ModelClubCellViewContenMain()
				{
					Filter = _filter,
					LeaderboardData = item,
					UserInfo = GetUserInfo()
				});
			}
			
			// ProcessMyRankInClub();
		}

		private ModelApiUser GetUserInfo()
		{
			var apiUser = FactoryApi.Get<ApiUser>();
			return apiUser.Data;
		}

		public static void OpenDetail(int clubId)
		{
			SpecialExtensionObserver.PostEvent(null,TypeGameEvent.ClubDetail, clubId);
			Signal.Send(StreamId.UI.ClubDetail);
		}
		
		private void OnInfo()
		{
			ControllerPopup.ShowInformation(Localization.Get(TextId.Club_Information));
		}
	}
}