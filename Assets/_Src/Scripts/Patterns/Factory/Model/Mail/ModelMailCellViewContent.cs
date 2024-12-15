using Game.Defines;

namespace Game.Model
{
	public class ModelMailCellViewContent : ModelMailCellView
	{
		public ModelApiMailData Mail;

		public ModelMailCellViewContent()
		{
			Type = MailCellViewType.Content;
		}
	}
}