using System;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewPackageEvent : ModelPartnerCellViewPackageCollab
	{
		public ModelPartnerCellViewPackageEvent()
		{
			Type = PartnerCellViewType.PackageEvent;
		}
	}
}