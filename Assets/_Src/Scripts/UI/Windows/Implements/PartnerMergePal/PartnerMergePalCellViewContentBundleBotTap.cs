using System.Collections;
using System.Collections.Generic;
using Game.Model;
using Game.UI;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalCellViewContentBundleBotTap : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private ItemEventBundleBotTap itemBundleBotTap;

        public override void SetData(AModelPartnerMergePalCellView data)
        {
            if (data is ModelPartnerMergePalCellViewContentBundleBotTap modelData)
            {
                itemBundleBotTap.SetData(modelData.Data);
            }
        }
    }
}
