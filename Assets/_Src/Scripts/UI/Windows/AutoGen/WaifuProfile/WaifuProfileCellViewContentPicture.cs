using System.Collections.Generic;
using Game.Model;
using UnityEngine;

namespace Game.UI
{
	public class WaifuProfileCellViewContentPicture : ESCellView<ModelWaifuProfileCellView>
	{
		[SerializeField] private List<WaifuProfileItemPicture> items;
		
		public override void SetData(ModelWaifuProfileCellView model)
		{
			if (model is ModelWaifuProfileCellViewContentPicture data)
			{
				for (int i = 0; i < items.Count; i++)
				{
					if (i < data.RowData.Count)
					{
						var ele = data.RowData[i];
						items[i].SetData(ele);
					}
					else
					{
						items[i].gameObject.SetActive(false);
					}
				}
			}
		}
	}
}