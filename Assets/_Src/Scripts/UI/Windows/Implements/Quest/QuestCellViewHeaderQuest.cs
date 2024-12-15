using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
	public class QuestCellViewHeaderQuest : ESCellView<ModelQuestCellView>
	{
		[SerializeField] private Image imageIcon;
		[SerializeField] private TMP_Text textTitle;

		public override void SetData(ModelQuestCellView model)
		{
			var data = model as ModelQuestCellViewHeaderQuest;

			// imageIcon.sprite = ControllerSprite.Instance.GetQuestHeaderIcon(data.QuestCategory);
			imageIcon.LoadSpriteAutoParseAsync("quest_header_icon_" + (int)data.QuestCategory);
			textTitle.text = data.Title.ToUpperCase();
		}
	}
}
