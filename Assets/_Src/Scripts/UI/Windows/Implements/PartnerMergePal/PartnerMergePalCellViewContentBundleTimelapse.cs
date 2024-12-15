using System.Collections;
using System.Collections.Generic;
using Game.Model;
using Game.UI;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalCellViewContentBundleTimelapse : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private RowItemEventBundleTimelapse rowItem;

        public override void SetData(AModelPartnerMergePalCellView data)
        {
            if (data is ModelPartnerMergePalCellViewContentBundleTimelapse modelData)
            {
                rowItem.LoadData(modelData.ListItemData);
            }
        }
    }
}
