using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewQuest : ModelPartnerCellView
	{
		public ModelApiQuestData Quest;

		public ModelPartnerCellViewQuest()
		{
			Type = PartnerCellViewType.Quest;
		}
	}
}