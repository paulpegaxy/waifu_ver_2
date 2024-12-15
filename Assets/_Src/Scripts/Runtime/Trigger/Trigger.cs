using System;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
	public abstract class Trigger
	{
		// protected Entity _owner;
		protected Action<ModelTriggerEventData> OnTrigger;

		// public void Bind(Entity owner)
		// {
		// 	_owner = owner;
		// }

		public abstract void Init(ModelTrigger model);
		public abstract void Register(Action<ModelTriggerEventData> callback);
		public abstract void Unregister();
	}

	public class FactoryTrigger : Factory<ModelTriggerType, Trigger>
	{
	}
}