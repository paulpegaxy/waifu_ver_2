using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Extensions;
using Game.Model;
using Game.Ton;
using Newtonsoft.Json;
using Template.Defines;
using UnityEngine;

namespace Game.Runtime
{
	[Factory(ApiType.Shop, true)]
	public class ApiShop : Api<ModelApiShop>
	{
		public async UniTask<List<ModelApiShopData>> Get()
		{
			var shop = await Get<List<ModelApiShopData>>("/v1/shop-new/all", "data");
			Data.Shop = shop;
			Data.Notification();

			return shop;
		}
		
		public async UniTask GetSubscriptions()
		{
			// var subscriptions = await Get<List<ModelApiShopSubscriptionData>>("/v1/shop-new/subscription", "data");
			// Data.Subscriptions = subscriptions;
			// await Get<string>("/v1/chat/subscription", "data");
			await Get<string>("/v1/chat/config", "data");
		}

		public async UniTask<List<ModelApiShopBuy>> GetOrder()
		{
			return await Get<List<ModelApiShopBuy>>($"/v1/shop-new/order-pending", "data", new { });
		}

		public async UniTask<ModelApiShopBuy> Buy(string bundle_id)
		{
			return await Post<ModelApiShopBuy>($"/v1/shop-new/buy", "data", new { bundle_id });
		}

		public async UniTask<ModelApiShopBuy> Buy(TypeShopItem type)
		{
			return await Buy(type.ToString().PascalToSnake());
		}

		public async UniTask<ModelApiShopBuyWithStar> BuyWithStar(string bundle_id)
		{
			return await Post<ModelApiShopBuyWithStar>("/v1/shop-new/request-buy-v2", "data",
				new
				{
					bundle_id,
					quantity = 1,
					club_id = 1,
					currency = "STAR"
				});
		}

		public async UniTask<string> BuySubscription(string sub_id)
		{
			return await Post<string>("/v1/user/telegram-subscription", "data", new {sub_id});
		}

		public async UniTask<ModelApiShopBuy> BuyWithTon(string bundle_id, int quantity = 1, TypeShopItem typeShopItem = TypeShopItem.None)
		{
			var order = await BuyRequest(bundle_id, quantity);


			if (!TONConnect.IsConnected)
				await TONConnect.ConnectWalletAsync();
			var hashTon = await TONConnect.SendTonAsync(order.ton_address, order.price, order.order_id, order.ValidUntilUnix);

			try
			{
				await VerifyTonOrder(hashTon);
			}
			catch (Exception e)
			{
			}

			if (typeShopItem != TypeShopItem.None)
			{
				SpecialExtensionShop.CheckSaveOnChainTransaction(TypeShopItem.PremiumFieren, true);
			}
			
			// var hash = await TONConnect.SendTonAsync(order.ton_address, order.price, order.order_id, order.ValidUntilUnix);
			
			
			
			await WaitingTransactionByHash(hashTon);
			
			return await VerifyTonOrder(hashTon);
		}

		public async UniTask<ModelApiShopBuy> BuyWithTon(TypeShopItem type, int quantity = 1)
		{
			return await BuyWithTon(type.ToString().PascalToSnake(), quantity);
		}

		// public async UniTask<ModelApiShopBuy> BoostWithTon(int club_id, int quantity)
		// {
		// 	var bundle_id = TypeShopItem.BoostClub.ToString().PascalToSnake();
		// 	var order = await BuyRequest(bundle_id, club_id, quantity);
		// 	var hash = await TONConnect.SendTonAsync(order.ton_address, order.price, order.order_id, order.ValidUntilUnix);
		//
		// 	await WaitingTransactionByHash(hash);
		//
		// 	return await VerifyTonOrder(hash);
		// }

		public async UniTask CheckOrder()
		{
			var wallet = TONConnect.Wallet;
			if (wallet == null) return;

			var orders = await GetOrder();
			if (orders.Count == 0)
			{
				ControllerPopup.ShowToast(Localization.Get(TextId.Shop_PendingOrder));
				return;
			}

			var data = await TonApi.GetMessageByAddress(TONConnect.Wallet.account.userFriendlyAddress, orders[0].ton_address);
			foreach (var order in orders)
			{
				var message = data.messages.Find(x => x.message_content.decoded.comment == order.order_id);
				if (message == null) continue;

				await VerifyTonOrder(message.hash);

				ControllerPopup.ShowToast(string.Format(Localization.Get(TextId.Shop_SuccessVerifyOrder), order.order_id));
			}
		}

		private UniTask<ModelApiShopBuy> VerifyTonOrder(string msg_hash)
		{
			return Post<ModelApiShopBuy>($"/v1/shop-new/verify-order-ton", "data", new { msg_hash });
		}

		private async UniTask<ModelApiShopBuy> BuyRequest(string bundle_id, int quantity = 1)
		{
			return await Post<ModelApiShopBuy>($"/v1/shop-new/request-buy", "data", new { bundle_id, quantity });
		}

		private async UniTask WaitingTransactionByHash(string hash, bool isUsingEvent = false)
		{
			var encodedHash = UnityWebRequest.EscapeURL(hash);
			var loopCount = 50;

			while (loopCount > 0)
			{
				try
				{
					await UniTask.Delay(TimeSpan.FromSeconds(3));

					if (isUsingEvent)
					{
						var data = await TonApi.GetEventsByMsgHash(encodedHash);
						if (data != null && data.Count > 0 && data[0].trace_info.IsCompleted()) break;
					}
					else
					{
						var data = await TonApi.GetTransactionByMsgHash(encodedHash);
						if (data != null && data.transactions.Count > 0) break;
					}
				}
				catch
				{
				}

				loopCount--;
				if (loopCount == 0)
				{
					break;
				}
			}
		}
	}
}