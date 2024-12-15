using Game.Defines;

namespace Game.Model
{
	public class ModelWaifuProfileCellViewHeader : ModelWaifuProfileCellView
	{
		public ModelApiEntityConfig Data;
		
		public ModelWaifuProfileCellViewHeader()
		{
			Type = WaifuProfileCellViewType.Header;
		}
	}
}