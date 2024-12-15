using System;
using System.Collections.Generic;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelQuestCellViewContentAchievement : ModelQuestCellView
    {
        public List<ModelApiQuestData> Quests;

        public ModelQuestCellViewContentAchievement()
        {
            Type = QuestCellViewType.ContentAchievement;
        }
    }
}