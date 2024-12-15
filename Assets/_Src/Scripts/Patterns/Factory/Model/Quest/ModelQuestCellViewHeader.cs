using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelQuestCellViewHeader : ModelQuestCellView
    {
        public string Title;

        public ModelQuestCellViewHeader()
        {
            Type = QuestCellViewType.Header;
        }
    }
}
