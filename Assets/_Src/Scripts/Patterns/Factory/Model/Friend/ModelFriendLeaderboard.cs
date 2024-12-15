using System;
using BreakInfinity;

namespace Game.Model
{
	[Serializable]
	public class ModelFriendLeaderboard
	{
		public string Name;
		public int Rank;
		public int FriendCount;
		public BigDouble Score;
	}
}