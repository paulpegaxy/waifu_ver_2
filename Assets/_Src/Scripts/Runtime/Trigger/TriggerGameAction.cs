using System;
using Game.Extensions;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
	[Factory(ModelTriggerType.GameAction)]
	public class TriggerGameAction : Trigger
	{
		private ModelTriggerGameAction _model;

		public override void Init(ModelTrigger model)
		{
			_model = model as ModelTriggerGameAction;
		}

		public override void Register(Action<ModelTriggerEventData> callback)
		{
			Pet.OnBuy += OnPetBuy;
			Pet.OnMerge += OnPetMerge;
			Pet.OnSell += OnPetSell;
			// Gameplay.OnFirstTapGirlTut += OnFirstTapGirl;
			this.RegisterEvent(TypeGameEvent.FristTapGirlTut,OnFirstTapGirl);
			// Gameplay.OnTapUndressGirlTut += OnNextConfirmUndressGirl;
			this.RegisterEvent(TypeGameEvent.UndressGirl, OnFirstUndressGirl);
			OnTrigger = callback;
		}

		private void OnPetBuy(int level)
		{
			if (_model.Action == GameAction.PetBuy)
			{
				OnTrigger?.Invoke(default);
			}
		}

		private void OnPetMerge(Pet source, Pet target)
		{
			if (_model.Action == GameAction.PetMerge)
			{
				OnTrigger?.Invoke(default);
			}
		}

		private void OnPetSell(Pet pet)
		{
			if (_model.Action == GameAction.PetSell)
			{
				OnTrigger?.Invoke(default);
			}
		}

		private void OnFirstTapGirl(object data)
		{
			if (_model.Action == GameAction.FirstTapGirl)
			{
				OnTrigger?.Invoke(default);
			}
		}

		private void OnFirstUndressGirl(object data)
		{
			if (_model.Action== GameAction.FirstUndressGirl)
			{
				OnTrigger?.Invoke(default);
			}
		}
		
		private void OnNextConfirmUndressGirl()
		{
			if (_model.Action == GameAction.TapToUndressGirl)
			{
				OnTrigger?.Invoke(default);
			}
		}

		public override void Unregister()
		{
			Pet.OnBuy -= OnPetBuy;
			Pet.OnMerge -= OnPetMerge;
			Pet.OnSell -= OnPetSell;
			// Gameplay.OnFirstTapGirlTut -= OnFirstTapGirl;
			// Gameplay.OnTapUndressGirlTut -= OnNextConfirmUndressGirl;
			this.RemoveEvent(TypeGameEvent.UndressGirl, OnFirstUndressGirl);
			this.RemoveEvent(TypeGameEvent.FristTapGirlTut,OnFirstTapGirl);
			OnTrigger = null;
		}
	}
}