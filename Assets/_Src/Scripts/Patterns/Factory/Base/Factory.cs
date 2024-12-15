using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
	[AttributeUsage(AttributeTargets.Class)]
	public class FactoryAttribute : Attribute
	{
		public readonly object _keyType;
		public readonly bool _isSingleton;
		public FactoryAttribute(object type, bool isSingleton = false)
		{
			_keyType = type;
			_isSingleton = isSingleton;
		}
	}

	public interface IFactory
	{
	}

	public class Factory<TKeyType, TModel> : IFactory where TKeyType : Enum
	{
		private static readonly Dictionary<string, Type> _database = new();
		private static readonly Dictionary<Type, TKeyType> _keys = new();
		private static readonly Dictionary<string, TModel> _instance = new();

		public static void Setup(List<Type> types)
		{
			CleanUp();
			
			foreach (Type type in types)
			{
				FactoryAttribute attribute = (FactoryAttribute)type.GetCustomAttributes(typeof(FactoryAttribute), false).FirstOrDefault();
				if (attribute != null && attribute._keyType.GetType() == typeof(TKeyType) && typeof(TModel).IsAssignableFrom(type))
				{
					string key = GetKey(typeof(TModel).Name, (TKeyType)attribute._keyType);
					if (!_database.ContainsKey(key))
					{
						_database.Add(key, type);
						_keys.Add(type, (TKeyType)attribute._keyType);
					}
				}
			}
		}

		public static T Get<T>() where T : TModel
		{
			if (!_keys.ContainsKey(typeof(T)))
			{
				return default;
			}

			return Get<T>(_keys[typeof(T)]);
		}

		public static T Get<T>(TKeyType type) where T : TModel
		{
			string key = GetKey(typeof(TModel).Name, type);
			if (_database.ContainsKey(key))
			{
				FactoryAttribute attribute = (FactoryAttribute) _database[key]
					.GetCustomAttributes(typeof(FactoryAttribute), false).FirstOrDefault();
				if (attribute._isSingleton)
				{
					if (!_instance.ContainsKey(key))
					{
						TModel instance = (TModel) Activator.CreateInstance(_database[key]);
						_instance.Add(key, instance);
					}

					return (T) _instance[key];
				}

				return (T) Activator.CreateInstance(_database[key]);
			}

			return default;
		}

		public static void CleanUp()
		{
			_database.Clear();
			_keys.Clear();
			_instance.Clear();
		}

		static string GetKey(string model, TKeyType type) => $"{model}_{typeof(TKeyType).Name}_{type}".ToLowerCase();
	}
}