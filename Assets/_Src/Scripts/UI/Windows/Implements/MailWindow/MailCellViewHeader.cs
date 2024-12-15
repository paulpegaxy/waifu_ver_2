using Game.Model;

namespace Game.UI
{
	public class MailCellViewHeader : ESCellView<ModelMailCellView>
	{
		public override void SetData(ModelMailCellView model)
		{
			var data = model as ModelMailCellViewHeader;
		}
	}
}