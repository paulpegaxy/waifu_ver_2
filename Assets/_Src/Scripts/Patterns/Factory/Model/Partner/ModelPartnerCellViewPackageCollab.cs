using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewPackageCollab : ModelPartnerCellView
	{
		public ModelApiShopData ShopData;

		public ModelPartnerCellViewPackageCollab()
		{
			Type = PartnerCellViewType.PackageCollab;
		}
	}
}