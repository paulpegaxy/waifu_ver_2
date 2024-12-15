using System.Collections;
using System.Collections.Generic;
using Game.Model;
using Game.UI;
using UnityEngine;

public class QuestCellViewContentQuestJackpot : ESCellView<ModelQuestCellView>
{
    [SerializeField] private QuestCellViewContentQuest itemQuest;
    
    public override void SetData(ModelQuestCellView model)
    {
        if (model is ModelQuestCellViewContentQuestJackpot data)
        {
            itemQuest.SetData(new ModelQuestCellViewContentQuest()
            {
                Quest = data.Quest
            });
        }
    }
}
