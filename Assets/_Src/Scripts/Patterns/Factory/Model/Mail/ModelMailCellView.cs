using Game.UI;
using Game.Defines;

namespace Game.Model
{
	public abstract class ModelMailCellView : IESModel<MailCellViewType>
	{
		public MailCellViewType Type { get; set; }
	}
	
	public enum MailCellViewType
	{
		Header,
		Content,
	}
}