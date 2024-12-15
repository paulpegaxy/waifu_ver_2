
using Game.UI;
using Template.Defines;

namespace Game.Model
{
    public abstract class ModelQuestCellView : IESModel<QuestCellViewType>
    {
        public QuestCellViewType Type { get; set; }
    }
    
    public enum QuestCellViewType
    {
        Header,
        HeaderQuest,
        HeaderAchievement,
        ContentQuest,
        ContentAchievement,
        ContentQuestJackpot
    }
}
