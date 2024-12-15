using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelQuestCellViewHeaderQuest : ModelQuestCellView
    {
        public QuestCategory QuestCategory;
        public string Title;

        public ModelQuestCellViewHeaderQuest()
        {
            Type = QuestCellViewType.HeaderQuest;
        }
    }
}
