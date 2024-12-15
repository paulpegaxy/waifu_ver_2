using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
	[Factory(ApiType.Leaderboard, true)]
	public class ApiLeaderboard : Api<ModelApiLeaderboard>
	{
		public async UniTask<ModelApiLeaderboardRank> Get(ModelClubFilter filter)
		{
			// if (filter.FilterType == FilterType.Personal)
			// {
				return await GetIndividual(filter);
			// }
			// else
			// {
			// 	return await GetClub();
			// }
		}
		
		public async UniTask<List<ModelApiLeaderboardClub>> GetClubs()
		{
			return await Get<List<ModelApiLeaderboardClub>>($"/v1/club/all", "data.data");
		}

		public async UniTask<ModelApiLeaderboardConfig> GetConfig()
		{
			var config = await Get<ModelApiLeaderboardConfig>($"/v1/leaderboard/configs", "data");
			config.AutoParseData();
			Data.Config = config;
			return config;
		}

		public async UniTask<ModelApiLeaderboardRank> GetIndividualAllTime()
		{
			return await Get<ModelApiLeaderboardRank>($"/v1/leaderboard/personal/point/all-time", "data");
		}

		private async UniTask<ModelApiLeaderboardRank> GetIndividual(ModelClubFilter filter)
		{
			var rank = filter.typeLeagueIndex;

			if (filter.FilterTimeType == FilterTimeType.Day)
			{
				return await Get<ModelApiLeaderboardRank>($"/v1/leaderboard/personal/point/day", "data", new { rank });
			}
			else if (filter.FilterTimeType == FilterTimeType.Week)
			{
				return await Get<ModelApiLeaderboardRank>($"/v1/leaderboard/personal/point/week", "data", new { rank });
			}
			else
			{
				return await Get<ModelApiLeaderboardRank>($"/v1/leaderboard/personal/point/all-time", "data",
					new { rank });
			}
		}

		// private async UniTask<ModelApiLeaderboardRank> GetClub()
		// {
		// 	var rank = filter.typeLeagueIndex;
		// 	// if (filter.FilterTimeType == FilterTimeType.Day)
		// 	// {
		// 	// 	return await Get<ModelApiLeaderboardRank>($"/v1/leaderboard/club/point/day", "data", new { rank });
		// 	// }
		// 	// else
		// 	// {
		// 	// 	return await Get<ModelApiLeaderboardRank>($"/v1/leaderboard/club/point/week", "data", new { rank });
		// 	// }
		// 	
		// 	
		// 	return await Get<ModelApiLeaderboardRank>($"/v1/club/all", "data", new {  });
		// }
	}
}