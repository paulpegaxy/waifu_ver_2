using System;
using Doozy.Runtime.UIManager.Containers;
using Newtonsoft.Json;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class ShopOfferSpecial : ShopOffer
	{
		private ShopOfferSpecialData _data;
		private bool _isShow;

		public override void Init(string type)
		{
			Type = ShopOfferType.Special;
			ItemType = type;
			CheckAndShow(ItemType);
		}

		public override void Show()
		{
			
		}

		public override void Update(float deltaTime)
		{
			if (TimeEnd > 0)
			{
				var duration = TimeEnd - ServiceTime.CurrentUnixTime;
				if (duration <= 0)
				{
					TimeEnd = 0;
					OnExpired?.Invoke(TypeShopItem.SpecialOffer1.ToString());
				}
			}
		}
		private void CheckAndShow(string id)
		{
			var apiShop = FactoryApi.Get<ApiShop>();

			Data = apiShop.Data.GetItemById(id);

			if (Data == null)
			{
				OnExpired?.Invoke(id);
				return;
			}

			if (Data.IsReachLimit)
			{
				OnExpired?.Invoke(id);
				return;
			}

			TimeEnd = ServiceTime.GetTimeRemain(Data.end_time);
			OnOffer?.Invoke(id);
			if (TimeEnd <= 0)
				OnExpired?.Invoke(id);
		}
	}

	[Serializable]
	public class ShopOfferSpecialData
	{
		public int date_time_start;
		public int date_time_end;
	}
}