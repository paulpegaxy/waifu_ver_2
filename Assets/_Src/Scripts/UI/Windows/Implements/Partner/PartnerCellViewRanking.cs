using UnityEngine;
using Game.Model;
using TMPro;
using Game.Runtime;

namespace Game.UI
{
	public class PartnerCellViewRanking : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private ItemRanking itemRanking;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text textScore;

		public override void SetData(ModelPartnerCellView model)
		{
			if (model is ModelPartnerCellViewRanking data)
			{
				itemRanking.SetData(data.Item.rank_top, data.Item.name, data.IsMine);
				textName.text = data.Item.name;
				textScore.text = data.Item.Gold.ToLetter();
			}
		}
	}
}