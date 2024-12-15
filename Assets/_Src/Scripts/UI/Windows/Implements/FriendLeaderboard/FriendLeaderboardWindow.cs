using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
	public class FriendLeaderboardWindow : UIWindow
	{
		[SerializeField] private FriendLeaderboardScroller scrollerLeaderboard;

		protected override async void OnEnabled()
		{
			this.ShowProcessing();
			try
			{
				var apiFriend = FactoryApi.Get<ApiFriend>();
				var leaderboard = await apiFriend.GetLeaderboard();

				var data = new List<ModelFriendLeaderboard>();
				foreach (var item in leaderboard.data)
				{
					data.Add(new ModelFriendLeaderboard()
					{
						Rank = item.rank,
						Name = item.user.name,
						FriendCount = item.total_invited,
						Score = item.BerryParse,
					});
				}
			
				scrollerLeaderboard.SetData(data);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}
	}
}