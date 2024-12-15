using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.Core;
using Template.Defines;

namespace Game.UI
{
	public class ShopPanelPal : MonoBehaviour
	{
		[SerializeField] private ShopScroller scroller;
		[SerializeField] private UIButton buttonRestore;
		[SerializeField] private GameObject objectButton;

		private ApiShop ApiShop => FactoryApi.Get<ApiShop>();

		private void OnEnable()
		{
			ModelApiShop.OnChanged += OnDataChanged;
			ModelApiUser.OnChanged+= OnUserChanged;
		}

		private void OnDisable()
		{
			ModelApiShop.OnChanged -= OnDataChanged;
			ModelApiUser.OnChanged-= OnUserChanged;
		}

		private void OnDataChanged(ModelApiShop data)
		{
			Refresh();
		}
		
		private void OnUserChanged(ModelApiUser data)
		{
			Refresh();
		}

		public void Refresh()
		{
			var listData = new List<ModelShopCellView>();

			// listData.Add(new ModelShopCellViewHeader());
			
			ProcessSubscriptionPack(ref listData);

			// listData.Add(new ModelShopCellViewHeaderNormalPack());

			ProcessListContent(ref listData);

			scroller.SetData(listData);
			// objectButton.SetActive(TONConnect.IsConnected);
		}

		private void ProcessSubscriptionPack(ref List<ModelShopCellView> listData)
		{
			var data = FactoryApi.Get<ApiChatInfo>().Data.GameConfig.subscription;
			var apiUser = FactoryApi.Get<ApiUser>().Data;
			
			for (int i = 0; i < data.Count; i++)
			{
				var ele = data[i];
				if (!apiUser.IsHaveSubscription(ele.id))
				{
					listData.Add(new ModelShopCellViewContentSubscription()
					{
						Config = ele,
						Index = i
					});
				}
			}
		}


		private void ProcessListContent(ref List<ModelShopCellView> listData)
		{
			var items = ApiShop.Data.GetItemsByPack(TypeShopPack.ShopBerryPack);
			var groupedItems = new List<List<ModelApiShopData>>();
			for (var i = 0; i < items.Count; i += 3)
			{
				var sublist = items.Skip(i).Take(3).ToList();
				groupedItems.Add(sublist);
			}

			foreach (var group in groupedItems)
			{
				listData.Add(new ModelShopCellViewContentNormal()
				{
					RowItemData = group
				});
			}
		}
	}
}