using System;
using System.Collections.Generic;
using BreakInfinity;

namespace Game.Model
{
	[Serializable]
	public class ModelApiFriendLeaderboard
	{
		public int offset;
		public int limit;
		public int total;
		public List<ModelApiFriendLeaderboardData> data;
	}

	[Serializable]
	public class ModelApiFriendLeaderboardData
	{
		public ModelApiUserData user;
		public int rank;
		public string berry;
		public int total_invited;

		public BigDouble BerryParse => BigDouble.Parse(berry);
	}
}