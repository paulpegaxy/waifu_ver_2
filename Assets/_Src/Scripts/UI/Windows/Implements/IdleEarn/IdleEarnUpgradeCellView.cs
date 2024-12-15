using System.Collections.Generic;
using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class IdleEarnUpgradeCellView :  ESCellView<DataIdleEarnUpgradeItem>
    {
        [SerializeField] private List<IdleEarnManageCard> items;

        public override void SetData(List<DataIdleEarnUpgradeItem> data, int index)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var info = index + i < data.Count ? data[index + i] : null;
                if (info != null)
                {
                    items[i].LoadData(info);
                }
                items[i].gameObject.SetActive(info != null);
            }
        }
    }
}