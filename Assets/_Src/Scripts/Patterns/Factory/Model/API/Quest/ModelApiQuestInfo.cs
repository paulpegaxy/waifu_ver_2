using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiQuestInfo
	{
		public ModelApiQuestStatistics achievement_summary;
		public List<ModelApiQuestData> quests;
	}
}
