using UnityEngine;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class JackpotCellViewHeader : ESCellView<AModelJackpotCellView>
	{
		[SerializeField] private JackpotHeader header;

		public override void SetData(AModelJackpotCellView model)
		{
			var data = model as ModelJackpotCellViewHeader;
			header.SetData(data.Jackpot);
		}
	}
}