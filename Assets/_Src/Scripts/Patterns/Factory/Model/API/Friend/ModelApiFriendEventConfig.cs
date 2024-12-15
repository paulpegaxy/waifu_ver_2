using System;
using System.Collections.Generic;
using Game.Defines;
using Game.Runtime;

namespace Game.Model
{
	[Serializable]
	public class ModelApiFriendEventConfig
	{
		public ModelApiFriendEventConfigData current;
		public ModelApiFriendEventConfigData pre;
		public ModelApiFriendEventConfigData last;
		public List<ModelApiItemData> total_rewards;

		public ModelApiFriendEventConfigData GetData(TypeFriendSeason season)
		{
			return season switch
			{
				TypeFriendSeason.Current => current,
				TypeFriendSeason.Prev => pre,
				_ => last,
			};
		}

		public bool HasEvent => current != null;
		// && current.IsAvailable || pre != null && pre.IsAvailable || last != null && last.IsAvailable;
	}

	[Serializable]
	public class ModelApiFriendEventConfigData
	{
		public int season_number;
		public long time_end;
		public long time_start;

		public bool IsAvailable => ServiceTime.CurrentUnixTime >= time_start && ServiceTime.CurrentUnixTime < time_end;
		public bool IsEnded => ServiceTime.CurrentUnixTime >= time_end;
	}
}