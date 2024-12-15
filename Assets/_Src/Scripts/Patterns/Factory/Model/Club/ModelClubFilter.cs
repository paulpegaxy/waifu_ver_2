using System;

namespace Game.Model
{
	[Serializable]
	public class ModelClubFilter
	{
		public FilterType FilterType;
		public FilterTimeType FilterTimeType;
		public int typeLeagueIndex;
	}
}