using UnityEngine;
using Game.Model;
using System.Collections.Generic;
using Game.Runtime;

namespace Game.UI
{
	public class BuddyCellViewContentLayer : ESCellView<ModelBuddyCellView>
	{
		[SerializeField] private List<BuddyInformation> items;

		private void OnEnable()
		{
			var apiFriend = FactoryApi.Get<ApiFriend>();
			var config = apiFriend.Data.Config.referral_layer_config;

			for (int i = 0; i < items.Count; i++)
			{
				items[i].SetData(config[i]);
			}
		}
	}
}
