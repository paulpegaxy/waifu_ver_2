using System.Collections;
using System.Collections.Generic;
using Game.Model;
using Game.UI;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalCellViewContentQuest : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private QuestCellViewContentQuest itemQuest;

        public override void SetData(AModelPartnerMergePalCellView data)
        {
            if (data is ModelPartnerMergePalCellViewContentQuest modelData)
            {
                itemQuest.SetData(new ModelQuestCellViewContentQuest()
                {
                    Quest = modelData.QuestData
                });
            }
        }
    }
}
