using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewBanner : ModelPartnerCellView
	{
		public ModelPartnerCellViewBanner()
		{
			Type = PartnerCellViewType.Banner;
		}
	}
}