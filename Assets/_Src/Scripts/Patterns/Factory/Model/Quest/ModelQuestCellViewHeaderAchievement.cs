using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelQuestCellViewHeaderAchievement : ModelQuestCellView
    {
        public QuestCategory QuestCategory;
        public string Title;
        public float TotalCompleted;

        public ModelQuestCellViewHeaderAchievement()
        {
            Type = QuestCellViewType.HeaderAchievement;
        }
    }
}