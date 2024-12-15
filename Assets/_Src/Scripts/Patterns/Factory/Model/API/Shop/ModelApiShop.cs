using System;
using System.Collections.Generic;
using System.Linq;
using Template.Defines;

namespace Game.Model
{
	[Serializable]
	public class ModelApiShop : ModelApiNotification<ModelApiShop>
	{
		public List<ModelApiShopData> Shop;
		public List<ModelApiShopSubscriptionData> Subscriptions;

		private Dictionary<TypeShopItem, ModelApiShopData> _dictShopItem;

		public ModelApiShopData GetItemById(string bundleId)
		{
			return Shop.Find(x => x.id == bundleId);
		}

		public ModelApiShopData GetItemByItemType(TypeShopItem type)
		{
			return _dictShopItem.TryGetValue(type, out var item) ? item : null;
		}

		public List<ModelApiShopData> GetItemsByPack(TypeShopPack pack)
		{
			return Shop.FindAll(x => x.GetPackType() == pack);
		}

		public List<ModelApiShopData> GetDailyItems()
		{
			return Shop.FindAll(x => x.GetPackType() == TypeShopPack.PeriodPack && x.buy_type == "daily");
		}

		public List<ModelApiShopData> GetWeeklyItems()
		{
			return Shop.FindAll(x => x.GetPackType() == TypeShopPack.PeriodPack && x.buy_type == "weekly");
		}

		public List<ModelApiShopData> GetListShopItemBasedEvent(string eventId)
		{
			return Shop.FindAll(x => x.IsMatchEventId(eventId));
		}

		public override void Notification()
		{
			_dictShopItem = Shop.GroupBy(item => item.GetItemType()).ToDictionary(group => group.Key, group => group.First());
			OnChanged?.Invoke(this);
		}
	}
}