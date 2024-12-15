using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class EventYukiCellViewContentBundleTimelapse: ESCellView<AModelEventYukiCellView>
    {
        [SerializeField] private RowItemEventBundleTimelapse rowItem;

        public override void SetData(AModelEventYukiCellView data)
        {
            if (data is ModelEventYukiCellViewContentBundleTimelapse modelData)
            {
                rowItem.LoadData(modelData.ListItemData);
            }
        }
    }
}