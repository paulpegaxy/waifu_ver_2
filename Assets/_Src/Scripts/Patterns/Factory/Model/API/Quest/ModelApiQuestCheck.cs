using System;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Game.Model
{
	[Serializable]
	public class ModelApiQuestCheck
	{
		public string id;
		public bool is_completed;
		public int end_time;

		public TimeSpan GetEndTimeSpan()
		{
			if (end_time > 0)
			{
				var time = end_time - ServiceTime.CurrentUnixTime;
				if (time > 0)
				{
					return TimeSpan.FromSeconds(time);
				}
			}

			return TimeSpan.Zero;
		}
	}
}
