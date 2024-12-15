using System;
using EnhancedUI.EnhancedScroller;
using Game.Model;
namespace Game.UI
{
	public class DatingScroller : ESMulti<DatingCellViewType, ModelDatingCellView, ESCellView<ModelDatingCellView>>
	{
		private const int SPACE_PER_MESS = 55;

		public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			var iData = _data[dataIndex] as IESModel<DatingCellViewType>;
			var size = _cellSize[Convert.ToInt32(iData.Type)];

			switch (iData.Type)
			{
				case DatingCellViewType.ContentOtherMesssage:
				case DatingCellViewType.COntentMyMessage:
					var modifyValue = CalculateCellHeightAdditive(_data[dataIndex].Message);
					return size.y + modifyValue;
			}

			return size.y;
		}

		private float CalculateCellHeightAdditive(string message)
		{
			float value = 0;
			var kvp = message.InsertLineBreaksByCharacters(GameConsts.MAX_COUNT_CHAR_CHAT);
			var count = kvp.Key + 1;
			if (count > 2)
			{
				// ReSharper disable once PossibleLossOfFraction
				value = SPACE_PER_MESS * (count- 2);
			}
			
			return value;
		}
	}
}