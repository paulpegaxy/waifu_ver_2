using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelQuestCellViewContentQuest : ModelQuestCellView
    {
        public ModelApiQuestData Quest;

        public ModelQuestCellViewContentQuest()
        {
            Type = QuestCellViewType.ContentQuest;
        }
    }
}
