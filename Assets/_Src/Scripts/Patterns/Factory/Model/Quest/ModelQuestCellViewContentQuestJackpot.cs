namespace Game.Model
{
    public class ModelQuestCellViewContentQuestJackpot : ModelQuestCellView
    {
        public ModelApiQuestData Quest;
        
        public ModelQuestCellViewContentQuestJackpot()
        {
            Type = QuestCellViewType.ContentQuestJackpot;
        }
    }
}