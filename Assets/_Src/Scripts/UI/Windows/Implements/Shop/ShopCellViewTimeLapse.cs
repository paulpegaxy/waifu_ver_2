
using System.Collections;
using System.Collections.Generic;using Game.Model;using Game.UI;
using UnityEngine;

public class ShopCellViewTimeLapse : ESCellView<ModelShopCellView>
{
    [SerializeField] private List<ShopItemTimeLapse> items;

    public override void SetData(ModelShopCellView model)
    {
        var data = model as ModelShopCellViewContentTimeLapse;
        if (data == null) return;
        
        int count = Mathf.Min(items.Count, data.RowItemData.Count);
        for (var i = 0; i < count; i++)
        {
            var itemData = data.RowItemData[i];
            items[i].SetData(itemData);
            items[i].gameObject.SetActive(itemData != null);
        }
			
        if (count < items.Count)
        {
            for (var i = count; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }
}
