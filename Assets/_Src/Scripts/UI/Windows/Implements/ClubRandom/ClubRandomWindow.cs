using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.Signals;
using Game.Core;
using Game.Extensions;
using Game.Runtime;
using Game.Model;
using Template.Defines;

namespace Game.UI
{
	public class ClubRandomWindow : UIWindow
	{
		[SerializeField] private ClubScroller _scrollerMain;
		[SerializeField] private UIButton buttonJoin;

		protected override void OnEnabled()
		{
			buttonJoin.onClickEvent.AddListener(OnJoin);
			Fetch();
		}

		protected override void OnDisabled()
		{
			buttonJoin.onClickEvent.RemoveListener(OnJoin);
		}

		private void OnJoin()
		{
			this.PostEvent(TypeGameEvent.Club, new ModelClubFilter()
			{
				FilterType = FilterType.Club,
				FilterTimeType = FilterTimeType.Day,
				typeLeagueIndex = (int) TypeLeague.Bronze,
			});
			Signal.Send(StreamId.UI.Club);
		}

		private async void Fetch()
		{
			this.ShowProcessing();
			try
			{
				var apiClub = FactoryApi.Get<ApiClub>();
				var clubs = await apiClub.Get();

				var data = new List<ModelClubCellView>
				{
					new ModelClubCellViewHeaderRandom()
				};

				foreach (var item in clubs.data)
				{
					data.Add(new ModelClubCellViewContentRandom() { Club = item });
				}

				_scrollerMain.SetData(data);
				this.HideProcessing();
			}
			catch (System.Exception e)
			{
				e.ShowError();
			}
		}
	}
}