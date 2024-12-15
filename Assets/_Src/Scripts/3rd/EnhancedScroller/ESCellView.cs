using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

namespace Game.UI
{
	public abstract class ESCellView<TModel> : EnhancedScrollerCellView
	{
		public virtual void SetData(TModel data) { }
		public virtual void SetData(List<TModel> data, int index) { }
	}
}
