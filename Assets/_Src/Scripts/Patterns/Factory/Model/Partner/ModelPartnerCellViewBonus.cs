using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewBonus : ModelPartnerCellView
	{
		public ModelPartnerCellViewBonus()
		{
			Type = PartnerCellViewType.Bonus;
		}
	}
}