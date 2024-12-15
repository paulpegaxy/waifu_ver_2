using System;
using Game.Model;

namespace Game.UI
{
    [Serializable]
    public class ModelEventYukiCellViewContentQuest : AModelEventYukiCellView
    {
        public ModelApiQuestData QuestData;
        
        public ModelEventYukiCellViewContentQuest()
        {
            Type = TypeEventYukiCellView.ContentQuest;
        }
    }
}