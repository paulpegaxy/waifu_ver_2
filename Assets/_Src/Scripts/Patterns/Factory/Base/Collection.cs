using System;
using UnityEngine;
using Newtonsoft.Json;
using Game.Model;
using Template.Utils;

namespace Game.Runtime
{
	public enum CollectionType
	{
		Rarities,
		GameConfig,
		Tutorial
	}

	public interface ICollection
	{
		void Init();
	}

	public abstract class Collection<TModel> : ICollection
	{
		protected TModel _model;
		protected string _key;

		public virtual void Init()
		{
#if UNITY_EDITOR
			TextAsset textAsset = Resources.Load<TextAsset>($"Collections/{_key}");
			_model = JsonConvert.DeserializeObject<TModel>(textAsset.text, new ModelConverter());
#else
			TextAsset textAsset = Resources.Load<TextAsset>($"Collections/{_key}");
			string json = Crypto.Decrypt(textAsset.bytes);

			_model = JsonConvert.DeserializeObject<TModel>(json, new ModelConverter());
#endif
		}

		public virtual TModel Get()
		{
			return _model;
		}

		protected string GetKey(CollectionType type)
		{
			return $"collection_{type.ToString().PascalToSnake()}";
		}
	}

	public class FactoryCollection : Factory<CollectionType, ICollection>
	{
		public static void Init()
		{
			int count = Enum.GetNames(typeof(CollectionType)).Length;
			for (int i = 0; i < count; i++)
			{
				Get<ICollection>((CollectionType)i).Init();
			}
		}
	}
}