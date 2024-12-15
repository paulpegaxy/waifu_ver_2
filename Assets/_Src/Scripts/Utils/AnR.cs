using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using Game.Runtime;
using Object = UnityEngine.Object;

public partial class AnR
{
	static readonly Dictionary<string, Object> _addressableAssets = new();
	private static readonly Dictionary<string, List<Object>> _addressableAssetsByLabel = new();
	static readonly Dictionary<string, Object> _resourceAssets = new();
	static readonly Dictionary<object, AsyncOperationHandle> _handler = new();

	public static async UniTask<T> LoadAddressable<T>(string key) where T : Object
	{
		if (!_addressableAssets.ContainsKey(key))
		{
			if (!_handler.ContainsKey(key))
			{
				var handler = Addressables.LoadAssetAsync<T>(key);
				_handler.Add(key, handler);
			}

			var asset = await _handler[key].Task as Object;
			if (!_addressableAssets.ContainsKey(key))
			{
				_addressableAssets.Add(key, asset);
			}
		}

		return _addressableAssets[key] as T;
	}

	public static async UniTask LoadAddressableByLabels<T>(List<string> labels, bool manualReleaseHandle = true) where T : Object
	{
		var handler = Addressables.LoadAssetsAsync<T>(labels, asset =>
		{
			string key = asset.name;
			if (!_addressableAssets.ContainsKey(key))
			{
				_addressableAssets.Add(key, asset);
			}
			else
			{
				_addressableAssets[key] = asset;
			}

			if (!_addressableAssetsByLabel.ContainsKey(labels[0]))
			{
				_addressableAssetsByLabel.Add(labels[0], new List<Object>() { asset });
			}
			else
			{
				_addressableAssetsByLabel[labels[0]].Add(asset);
			}


		}, Addressables.MergeMode.Union);

		if (manualReleaseHandle)
		{
			_handler.Add(labels, handler);
		}

		await handler.Task;
	}

	public static List<T> GetAllAssetsByLabel<T>(string label) where T : Object
	{
		if (_addressableAssetsByLabel.TryGetValue(label, out var assets))
		{
			return assets.Cast<T>().ToList();
		}

		return null;
	}

	public static async UniTask<SceneInstance> LoadScene(string key, LoadSceneMode mode = LoadSceneMode.Additive)
	{
		var handler = Addressables.LoadSceneAsync(key, mode);
		var sceneInstance = await handler.Task;

		_handler.Add(key, handler);

		return sceneInstance;
	}

	public static async UniTask UnloadScene(string key)
	{
		if (_handler.ContainsKey(key))
		{
			await Addressables.UnloadSceneAsync(_handler[key]);
			_handler.Remove(key);
		}
	}

	public static async UniTask<T> LoadResource<T>(string path) where T : Object
	{
		var key = Path.GetFileName(path);
		if (!_resourceAssets.ContainsKey(key))
		{
			var asset = await Resources.LoadAsync<T>(path);
			_resourceAssets.Add(key, asset);
		}

		return _resourceAssets[key] as T;
	}

	public static void LoadResourceByFolder<T>(params string[] paths) where T : Object
	{
		foreach (var path in paths)
		{
			var assets = Resources.LoadAll<T>(path);
			foreach (var asset in assets)
			{
				var key = Path.GetFileName(asset.name);
				_resourceAssets.Add(key, asset);
			}
		}
	}

	public static T Get<T>(string key) where T : Object
	{
		if (_resourceAssets.ContainsKey(key))
		{
			return _resourceAssets[key] as T;
		}
		
		if (_addressableAssets.ContainsKey(key))
		{
			return _addressableAssets[key] as T;
		}
		
		return null;
	}

	public static T Get<T>(System.Enum id) where T : Object
	{
		return Get<T>(GetKey(id));
	}

	public static async UniTask<T> GetAsync<T>(string key) where T : Object
	{
		var asset = Get<T>(key);
		if (asset != null)
		{
			return asset;
		}

		return await LoadAddressable<T>(key);
	}

	public static async UniTask<T> CheckAssetIsAvailable<T>(string key) where T: Object
	{
		var keyParse = GetKeyParse(key);
		var value = await GetAsync<T>(keyParse);
		return value;
	}

	public static async UniTask<T> GetAsync<T>(System.Enum id) where T : Object
	{
		return await GetAsync<T>(GetKey(id));
	}

	public static string GetKey(System.Enum id)
	{
		return id.ToString().PascalToSnake();
	}

	public static SpriteKey GetKeyParse(string key)
	{
		return (AnR.SpriteKey)Enum.Parse(typeof(AnR.SpriteKey), key);
	}

	public static void ReleaseAll()
	{
		foreach (var handler in _handler.Values)
		{
			Addressables.Release(handler);
		}

		_handler.Clear();
		_resourceAssets.Clear();
	}
}
