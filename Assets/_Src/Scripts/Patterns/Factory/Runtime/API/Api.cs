using System;
using System.Text;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game.Runtime
{
	public enum ApiType
	{
		Common,
		User,
		Game,
		Shop,
		Friend,
		Leaderboard,
		Entity,
		Quest,
		UpgradeInfo,
		Club,
		Event,
		Tracking,
		Mail,
		ChatAI,
		EntityMatched,
		ChatInfo
	}

	public interface IApi
	{

	}

	public abstract class Api<TModel> : IApi
	{
		protected TModel _model = (TModel)Activator.CreateInstance(typeof(TModel));
		public TModel Data => _model;

		private string _baseUrl = "https://api-dev.waifutap.io";

		public Api()
		{

#if PRODUCTION_BUILD
			_baseUrl = "https://api.pocketwaifu.io";
#endif
		}

		protected async UniTask<T> Get<T>(string path, string selectToken = "", object data = null, bool isShowException = true)
		{
			var jsonStr = JsonConvert.SerializeObject(data);
			var request = UnityWebRequest.Get($"{_baseUrl}{path}?{data.ToParams()}");
			ProcessHeader(request);

			GameUtils.Log("green", $"{request.method} {request.uri}");

			try
			{
				await request.SendWebRequest();
				var response = HandlerRequest(request, jsonStr);

				if (!string.IsNullOrEmpty(response))
				{
					return response.Parse<T>(selectToken);
				}
				else
				{
					return default;
				}
			}
			catch (Exception e)
			{
				e.ShowError(isShowException);
				throw;
			}
		}

		protected async UniTask<T> Post<T>(string path, string selectToken = "", object data = null, bool isShowException = true)
		{
			var jObj = new JObject
			{
				{ "data", GameUtils.GetText(data ?? new { }) }
			};
			
			var jsonStr = JsonConvert.SerializeObject(jObj);
			var request = new UnityWebRequest($"{_baseUrl}{path}", "POST")
			{
				uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonStr)),
				downloadHandler = new DownloadHandlerBuffer()
			};
			ProcessHeader(request);

			GameUtils.Log("green", $"{request.method} {request.uri} {JsonConvert.SerializeObject(data)}");

			try
			{
				await request.SendWebRequest();
				var response = HandlerRequest(request, jsonStr);

				if (!string.IsNullOrEmpty(response))
				{
					return response.Parse<T>(selectToken);
				}
				else
				{
					return default;
				}
			}
			catch (Exception e)
			{
				e.ShowError(isShowException);
				throw;
			}
		}
		
		protected async UniTask<T> ManualGet<T>(string uri, string selectToken = "", object data = null, bool isShowException = true)
		{
			var jsonStr = JsonConvert.SerializeObject(data);
			var request = UnityWebRequest.Get($"{uri}?{data.ToParams()}");
			ProcessHeader(request);

			GameUtils.Log("green", $"{request.method} {request.uri}");

			try
			{
				await request.SendWebRequest();
				var response = HandlerRequest(request, jsonStr);

				if (!string.IsNullOrEmpty(response))
				{
					return response.Parse<T>(selectToken);
				}
				else
				{
					return default;
				}
			}
			catch (Exception e)
			{
				e.ShowError(isShowException);
				throw;
			}
		}

		protected async UniTask<T> ManualPost<T>(string uri, string selectToken = "", object data = null, bool isShowException = true,bool isUseBearerToken=false)
		{
			var jsonStr = JsonConvert.SerializeObject(data);
			var request = new UnityWebRequest(uri, "POST")
			{
				uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonStr)),
				downloadHandler = new DownloadHandlerBuffer()
			};
			ProcessHeader(request,isUseBearerToken);

			GameUtils.Log("green", $"{request.method} {request.uri} {jsonStr}");

			try
			{
				await request.SendWebRequest();
				var response = HandlerRequest(request, jsonStr);

				if (!string.IsNullOrEmpty(response))
				{
					return response.Parse<T>(selectToken);
				}
				else
				{
					return default;
				}
			}
			catch (Exception e)
			{
				e.ShowError(isShowException);
				throw;
			}
		}

		private void ProcessHeader(UnityWebRequest request, bool isUseBearerToken = false)
		{
			var api = FactoryApi.Get<ApiGame>();
			if (api.Data.Login != null)
			{
				if (isUseBearerToken)
				{
					request.SetRequestHeader("Authorization", "Bearer " + api.Data.Login.access_token);
				}
				else
				{
					request.SetRequestHeader("Authorization", api.Data.Login.access_token);
				}
			}
			
	

			request.SetRequestHeader("Content-Type", "application/json");
		}

		private string HandlerRequest(UnityWebRequest request, string body)
		{
			var url = request.uri.ToString();

			switch (request.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					var error = GameUtils.Parse(request.error);
					GameUtils.Log("red", $"{url} Error: {error}");
					break;

				case UnityWebRequest.Result.ProtocolError:
					var errorProtocol = GameUtils.Parse(request.error);
					GameUtils.Log("red", $"{url} HTTP Error: {errorProtocol}");
					break;

				case UnityWebRequest.Result.Success:
					var response = GameUtils.Parse(request.downloadHandler.text);
					if (url.Contains("quest") || url.Contains("shop/all"))
					{
						
					}
					// UnityEngine.Debug.LogError("resposon "+request.downloadHandler.text);
					GameUtils.Log("yellow", $"{url} {body} Received:\n{response}");
					return response;
			}

			return null;
		}


	}

	public class FactoryApi : Factory<ApiType, IApi>
	{
	}
}