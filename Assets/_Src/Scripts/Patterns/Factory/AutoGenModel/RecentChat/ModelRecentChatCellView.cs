using Game.UI;
using Game.Defines;

namespace Game.Model
{
	public abstract class ModelRecentChatCellView : IESModel<RecentChatCellViewType>
	{
		public RecentChatCellViewType Type { get; set; }
	}
}