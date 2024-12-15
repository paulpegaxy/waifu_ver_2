using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewRanking : ModelPartnerCellView
	{
		public ModelApiEventRankingData Item;
		public bool IsMine;

		public ModelPartnerCellViewRanking()
		{
			Type = PartnerCellViewType.Ranking;
		}
	}
}