using System;
using System.Collections.Generic;
using Game.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelPartnerCellViewClaim : ModelPartnerCellView
	{
		public ModelPartnerCellViewClaim()
		{
			Type = PartnerCellViewType.Claim;
		}
	}
}