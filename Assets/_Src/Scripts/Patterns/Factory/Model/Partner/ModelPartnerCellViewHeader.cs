using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewHeader : ModelPartnerCellView
	{
		public string Title;

		public ModelPartnerCellViewHeader()
		{
			Type = PartnerCellViewType.Header;
		}
	}
}