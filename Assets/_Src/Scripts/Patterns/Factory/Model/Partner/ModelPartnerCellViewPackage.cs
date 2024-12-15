using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewPackage : ModelPartnerCellView
	{
		public int Index;
		public ModelApiShopData ShopData;

		public ModelPartnerCellViewPackage()
		{
			Type = PartnerCellViewType.Package;
		}
	}
}