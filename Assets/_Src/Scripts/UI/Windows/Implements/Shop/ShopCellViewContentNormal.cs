using System.Collections.Generic;
using UnityEngine;
using Game.Model;

namespace Game.UI
{
	public class ShopCellViewContentNormal : ESCellView<ModelShopCellView>
	{
		[SerializeField] private List<ShopItem> items;

		public override void SetData(ModelShopCellView model)
		{
			var data = model as ModelShopCellViewContentNormal;
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
		
		
		// public override void SetData(List<ModelApiShopData> data, int index)
		// {
		// 	for (var i = 0; i < items.Count; i++)
		// 	{
		// 		var info = index + i < data.Count ? data[index + i] : null;
		// 		if (info != null)
		// 		{
		// 			items[i].SetData(info);
		// 		}
		// 		items[i].gameObject.SetActive(info != null);
		// 	}
		// }
	}
}