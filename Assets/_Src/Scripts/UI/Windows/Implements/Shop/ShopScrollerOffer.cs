using EnhancedUI.EnhancedScroller;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
	public class ShopScrollerOffer : ESMulti<ShopOfferType, ModelShopOfferCellView, ESCellView<ModelShopOfferCellView>>
	{
		public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			return ControllerUI.Instance.GetWidth();
		}
	}
}
