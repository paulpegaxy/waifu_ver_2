using UnityEngine;
using Game.Model;

namespace Game.UI
{
	public class PartnerCellViewQuest : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private QuestCellViewContentQuest quest;

		public override void SetData(ModelPartnerCellView model)
		{
			var data = model as ModelPartnerCellViewQuest;
			quest.SetData(new ModelQuestCellViewContentQuest() { Quest = data.Quest });
		}
	}
}