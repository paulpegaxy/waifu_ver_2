using System;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public abstract class ShopOffer
	{
		public ShopOfferType Type;
		public string ItemType;
		public string ItemId;
		public ModelApiShopData Data;
		
		public static Action<string> OnOffer;
		public static Action<string> OnExpired;

		protected float TimeEnd;
		
		public abstract void Init(string type);

		public virtual void Reset()
		{
			
		}

		public virtual void Show()
		{
		}

		public virtual void Update(float deltaTime)
		{	if (TimeEnd > 0)
			{
				var duration = TimeEnd - ServiceTime.CurrentUnixTime;
				if (duration <= 0)
				{
					TimeEnd = 0;
					OnExpired?.Invoke(TypeShopItem.SpecialOffer1.ToString());
				}
			}
		}


		public virtual bool IsExpired()
		{
			return TimeEnd <= 0;
		}

		public virtual void Expired()
		{
			OnExpired?.Invoke("");
		}

		public void Refresh()
		{
			CheckAndShow(Data.id);
		}
		
		protected void CheckAndShow(string id)
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