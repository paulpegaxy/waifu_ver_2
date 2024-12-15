using Game.UI;
using Game.Defines;

namespace Game.Model
{
	public abstract class ModelWaifuProfileCellView : IESModel<WaifuProfileCellViewType>
	{
		public WaifuProfileCellViewType Type { get; set; }
	}
}