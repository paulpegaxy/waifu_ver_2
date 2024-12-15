using Game.Defines;

namespace Game.Model
{
	public class ModelRecentChatCellViewContent : ModelRecentChatCellView
	{
		public ModelApiChatHistory Data;
		
		public ModelRecentChatCellViewContent()
		{
			Type = RecentChatCellViewType.Content;
		}
	}
}