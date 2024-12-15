using System;
using System.Collections.Generic;
using BreakInfinity;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
	public static class ControllerResource
	{
		public static Action<TypeResource, BigDouble, BigDouble> OnChanged;

		private static readonly Dictionary<TypeResource, ModelResource> _resources = new();

		public static ModelResource Get(TypeResource type)
		{
			if (!_resources.ContainsKey(type))
			{
				_resources[type] = new ModelResource { Type = type, Amount = 0 };
			}

			return _resources[type];
		}

		public static ModelResource Set(TypeResource type, BigDouble amount)
		{
			// UnityEngine.Debug.LogError("Set resource: " + type + " amount: " + amount);
			var resource = Get(type);
			var oldAmount = resource.Amount;
			var newAmount = amount;
			
			resource.Amount = newAmount;

			if (oldAmount != newAmount)
				OnChanged?.Invoke(type, oldAmount, newAmount);

			return resource;
		}

		public static ModelResource Add(TypeResource type, BigDouble amount)
		{
			var resource = Get(type);
			var oldAmount = resource.Amount;
			var newAmount = oldAmount + amount;

			resource.Amount = newAmount;

			OnChanged?.Invoke(type, oldAmount, newAmount);

			return resource;
		}

		public static ModelResource Subtract(TypeResource type, BigDouble amount)
		{
			var resource = Get(type);
			var oldAmount = resource.Amount;
			var newAmount = oldAmount - amount;

			resource.Amount = newAmount;

			OnChanged?.Invoke(type, oldAmount, newAmount);

			return resource;
		}

		public static bool IsEnough(TypeResource type, BigDouble amount)
		{
			var resource = Get(type);
			return resource.Amount >= amount;
		}
	}
}