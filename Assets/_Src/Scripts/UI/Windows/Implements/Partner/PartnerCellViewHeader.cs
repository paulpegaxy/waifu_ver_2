using UnityEngine;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class PartnerCellViewHeader : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private TMP_Text textTitle;

		public override void SetData(ModelPartnerCellView model)
		{
			var data = model as ModelPartnerCellViewHeader;
			textTitle.text = data.Title;
		}
	}
}