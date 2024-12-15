using UnityEngine;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class QuestCellViewHeader : ESCellView<ModelQuestCellView>
	{
		[SerializeField] private TMP_Text textTitle;

		public override void SetData(ModelQuestCellView model)
		{
			var data = model as ModelQuestCellViewHeader;
			textTitle.text = data.Title.ToUpper();
		}
	}
}
