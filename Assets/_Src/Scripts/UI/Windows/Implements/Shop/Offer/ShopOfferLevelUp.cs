using System;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Newtonsoft.Json;
using Template.Defines;

namespace Game.UI
{
	public class ShopOfferLevelUp : ShopOffer
	{

		public override void Init(string type)
		{
			Type = ShopOfferType.LevelUp;
			ItemType = type;
			// ModelApiGameInfo.OnChanged += OnGameInfoChanged;
			Show();
		}

		public override void Show()
		{
			var apiGame = FactoryApi.Get<ApiGame>();
			CheckAndShow(ItemType, apiGame.Data.Info.current_level_girl);
		}

		public override void Update(float deltaTime)
		{
			if (TimeEnd > 0)
			{
				var duration = TimeEnd - ServiceTime.CurrentUnixTime;
				if (duration <= 0)
				{
					TimeEnd = 0;
					OnExpired?.Invoke(TypeShopItem.LevelUpOffer1.ToString());
				}
			}
		}

		private void CheckAndShow(string id, int level)
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
}

[Serializable]
public class ShopOfferLevelUpData
{
	public int start_level;
	public int end_level;
	public int range_boost_level;
	public int rate_bonus;
	public int duration;
}