using System;
using UnityEngine;
using Newtonsoft.Json;
using Game.Model;

namespace Game.Runtime
{
	public enum StorageType
	{
		Settings,
		UserInfo,
		Tutorial,
		Shopping,
	}

	public interface IStorage
	{
		void Init();
		void Save();
		void Load();
		void Load(string json);
		string GetData();
	}

	public class Storage<TModel> : IStorage
	{
		protected TModel _model;
		protected string _key;

		public void Init()
		{
			if (PlayerPrefs.HasKey(_key))
			{
				Load();
			}
			else
			{
				_model = (TModel)Activator.CreateInstance(typeof(TModel));
				InitModel();
			}
		}
		public virtual void Save()
		{
			if (_model != null)
			{
				string json = JsonConvert.SerializeObject(_model);
				PlayerPrefs.SetString(_key, json);
			}
		}

		public virtual void Load()
		{
			string json = PlayerPrefs.GetString(_key);
			_model = JsonConvert.DeserializeObject<TModel>(json, new ModelConverter());
		}

		public virtual void Load(string json)
		{
			_model = JsonConvert.DeserializeObject<TModel>(json, new ModelConverter());
		}

		public virtual TModel Get()
		{
			return _model;
		}

		public string GetData()
		{
			return JsonConvert.SerializeObject(_model, Formatting.None);
		}

		protected virtual void InitModel()
		{

		}

		protected string GetKey(StorageType type)
		{
			return $"storage_{type.ToString().PascalToSnake()}";
		}
	}

	public class FactoryStorage : Factory<StorageType, IStorage>
	{
		public static void Init()
		{
			int count = Enum.GetNames(typeof(StorageType)).Length;
			for (int i = 0; i < count; i++)
			{
				Get<IStorage>((StorageType)i).Init();
			}
		}

		public static void SaveAll()
		{
			int count = Enum.GetNames(typeof(StorageType)).Length;
			for (int i = 0; i < count; i++)
			{
				IStorage storage = Get<IStorage>((StorageType)i);
				storage?.Save();
			}

			PlayerPrefs.Save();
		}
	}
}