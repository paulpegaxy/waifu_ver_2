using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewBitgetInfo : ModelPartnerCellView
	{
		public ModelPartnerCellViewBitgetInfo()
		{
			Type = PartnerCellViewType.BitgetInfo;
		}
	}
}