using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiFriendEventLeaderboard
	{
		public List<ModelApiFriendEventLeaderboardData> list;
		public ModelApiFriendEventLeaderboardData current_user;
	}

	[Serializable]
	public class ModelApiFriendEventLeaderboardData
	{
		public ModelApiFriendEventLeaderboardUser user;
		public int rank;
		public int invited;
		public List<ModelApiItemData> items;

	}

	[Serializable]
	public class ModelApiFriendEventLeaderboardUser
	{
		public int id;
		public string name;
	}
}