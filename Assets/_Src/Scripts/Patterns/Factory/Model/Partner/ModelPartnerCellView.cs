using System;
using Game.Defines;
using Game.UI;

namespace Game.Model
{
	[Serializable]
	public abstract class ModelPartnerCellView : IESModel<PartnerCellViewType>
	{
		public PartnerCellViewType Type { get; set; }
		public ModelApiEventConfig Config;
	}
	
	public enum PartnerCellViewType
	{
		Banner,
		Header,
		Bonus,
		Quest,
		Package,
		PackageCollab,
		PackageEvent,
		Checkin,
		Claim,
		Ranking,
		BitgetInfo,
	}
	
	public enum PartnerCheckinStatus
	{
		Available,
		Claimed,
	}
}