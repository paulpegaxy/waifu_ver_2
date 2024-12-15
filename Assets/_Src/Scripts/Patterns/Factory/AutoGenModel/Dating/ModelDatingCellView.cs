using Game.UI;
using Game.Defines;

namespace Game.Model
{
	public abstract class ModelDatingCellView : IESModel<DatingCellViewType>
	{
		public string Message;
		
		public DatingCellViewType Type { get; set; }
	}
}