using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class EventYukiCellViewContentBundleBotTap: ESCellView<AModelEventYukiCellView>
    {
        [SerializeField] private ItemEventBundleBotTap itemBundleBotTap;

        public override void SetData(AModelEventYukiCellView data)
        {
            if (data is ModelEventYukiCellViewContentBundleBotTap modelData)
            {
                itemBundleBotTap.SetData(modelData.Data);
            }
        }
    }
}