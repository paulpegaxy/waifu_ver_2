using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class EventYukiCellViewContentQuest: ESCellView<AModelEventYukiCellView>
    {
        [SerializeField] private QuestCellViewContentQuest itemQuest;

        public override void SetData(AModelEventYukiCellView data)
        {
            if (data is ModelEventYukiCellViewContentQuest modelData)
            {
                itemQuest.SetData(new ModelQuestCellViewContentQuest()
                {
                    Quest = modelData.QuestData
                });
            }
        }
    }
}