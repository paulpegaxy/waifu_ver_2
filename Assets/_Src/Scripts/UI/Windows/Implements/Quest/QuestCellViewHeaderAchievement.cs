using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class QuestCellViewHeaderAchievement : ESCellView<ModelQuestCellView>
	{
		[SerializeField] private Image imageBackground;
		[SerializeField] private Image imageForeground;
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private TMP_Text txtTitleTotal;
		[SerializeField] private TMP_Text textTotalCompleted;
		[SerializeField] private List<Color> colorBackground;

		public override void SetData(ModelQuestCellView model)
		{
			var data = model as ModelQuestCellViewHeaderAchievement;
			var index = data.QuestCategory - QuestCategory.Checkin;

		
			if (data.QuestCategory == QuestCategory.Purchase)
			{
				textTotalCompleted.text = $"${data.TotalCompleted.ToDigit()}";
			}
			else
			{
				textTotalCompleted.text = data.TotalCompleted.ToString();
			}

			imageBackground.color = colorBackground[index ];
			// imageForeground.color = colorBackground[index * 2 + 1];

			txtTitleTotal.text = data.QuestCategory.ToQuestTitleTotal();
			txtTitleTotal.color = colorBackground[index];
			textTitle.text = data.QuestCategory.ToQuestAchievementHeader();
		}
	}
}
