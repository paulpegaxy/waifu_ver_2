using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Game.Runtime;
using Game.Model;

namespace Game.Ton
{
	public class TonApi
	{
#if PRODUCTION_BUILD
		private static string _baseUrl = "https://toncenter.com/api/v3";
#else
		private static string _baseUrl = "https://testnet.toncenter.com/api/v3";
#endif
		public static async UniTask<TonApiTransactions> WaitingTransactionByHash(string hash)
		{
			var encodedHash = UnityWebRequest.EscapeURL(hash);
			var loopCount = 50;

			while (loopCount > 0)
			{
				try
				{
					await UniTask.Delay(TimeSpan.FromSeconds(3));

					var data = await GetTransactionByMsgHash(encodedHash);
					if (data != null && data.transactions.Count > 0)
					{
						return data;
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

			return null;
		}

		public static UniTask<TonApiTransactions> GetTransactionByMsgHash(string msg_hash, string direction = "in", int limit = 128, int offset = 0)
		{
			return Get<TonApiTransactions>("/transactionsByMessage", "", new { msg_hash, direction, limit, offset });
		}

		public static UniTask<List<TonApiEvent>> GetEventsByMsgHash(string msg_hash)
		{
#if PRODUCTION_BUILD

			return Get<List<TonApiEvent>>("https://preview.toncenter.com/api/v3/events", "events", new { msg_hash });
#else
			return Get<List<TonApiEvent>>("/events", "events", new { msg_hash });
#endif

		}

		public static async UniTask<string> GetJettonWallet(string jettonMaster, string jettonOnwer)
		{
			var res = await Get<List<TonApiJettonWallet>>("/jetton/wallets", "jetton_wallets", new
			{
				owner_address = UnityWebRequest.EscapeURL(jettonOnwer),
				jetton_address = UnityWebRequest.EscapeURL(jettonMaster)
			});
			return res is { Count: > 0 } ? res[0].address : null;
		}

		public static UniTask<TonApiMessages> GetMessageByAddress(string source, string destination, int limit = 128, int offset = 0)
		{
			source = UnityWebRequest.EscapeURL(source);
			return Get<TonApiMessages>("/messages", "", new { source, destination, limit, offset });
		}

		public static async UniTask<string> Claim(ModelApiClaim data)
		{
			try
			{
				var msgHash = await TONConnect.ClaimTonAsync(JsonConvert.SerializeObject(data));
				var headTransactions = await WaitingTransactionByHash(msgHash);
				var outHashHeadTransaction = headTransactions.transactions[0].out_msgs[0].hash;
				var nextTransactions = await WaitingTransactionByHash(outHashHeadTransaction);

				var bounced = nextTransactions.transactions[0].out_msgs[0].bounced;
				if (bounced != null && bounced.Value)
				{
					return null;
				}
				else
				{
					return outHashHeadTransaction;
				}
			}
			catch
			{
				return null;
			}
		}

		private static async UniTask<T> Get<T>(string path, string selectToken = "", object data = null)
		{
			var url = path.StartsWith("http") ? path : $"{_baseUrl}{path}";
			var request = UnityWebRequest.Get($"{url}?{data.ToParams()}");

			request.SetRequestHeader("Content-Type", "application/json");

			try
			{
				UnityEngine.Debug.Log($"{request.method} {request.uri}");
				await request.SendWebRequest();
				if (request.result == UnityWebRequest.Result.Success)
				{
					var response = request.downloadHandler.text;
					return response.Parse<T>(selectToken);
				}
				else
				{
					return default;
				}
			}
			catch
			{
				throw;
			}
		}
	}
}