using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewCheckin : ModelPartnerCellView
	{
		public ModelPartnerCellViewCheckin()
		{
			Type = PartnerCellViewType.Checkin;
		}
	}
}