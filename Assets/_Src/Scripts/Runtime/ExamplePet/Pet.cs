using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

namespace Game.Runtime
{
	public class Pet : Entity
	{
		public static Action<int> OnBuy;
		public static Action<Pet> OnSell;
		public static Action<Pet, Pet> OnMerge;
		public static Action<int> OnLevelUp;
		public static Action<Pet> OnClick;
		public static Action<Pet, Vector2Int, Vector2Int> OnMove;

		public EntityTransform Transform { get; private set; }
		public EntityAnimation Animation { get; private set; }
		public PetStateMachine StateMachine { get; private set; }

		private GameObject _mesh;

		private void Update()
		{
			StateMachine?.Update();
		}

		public override void Init(int id,Action onComplete=null)
		{
			// _mesh = ControllerSpawner.Instance.Spawn($"{model.Id}_{AnR.GetKey(AnR.PetKey.Entity)}");
			// _mesh.SetActive(true);

			Transform = new EntityTransform(this);
			Transform.Attach(_mesh);

			Animation = new EntityAnimation(this);
			Animation.Init(_mesh.GetComponent<Animator>());

			StateMachine = new PetStateMachine(this);
			StateMachine.SetState(PetStateType.Idle);

			// Model = model;
		}

		public override void Reset()
		{
			Transform.DetachAll();
			StateMachine.Exit();

			// ControllerSpawner.Instance.Return(gameObject);
		}

		public bool CanMerge(Pet pet)
		{
			// return pet != this && EntityData.level == pet.EntityData.level;
			return false;
		}

		public void Thinking()
		{
			StateMachine.SetState(PetStateType.Thinking);
		}

		public void ResetPosition()
		{
			// if (Placement != null)
			// {
			// 	Placement.Place(this);
			// }
		}

		protected override void SetData(int id)
		{
			// if (data.GameId == (int)typene.PetFree) return;
			// if (Data != null && Data.GameId != data.GameId)
			// {
			// 	var mesh = ControllerSpawner.Instance.Spawn($"{data.GameId}_{AnR.GetKey(AnR.PetKey.Entity)}");
			// 	if (mesh != null)
			// 	{
			// 		ControllerSpawner.Instance.Return(_mesh);
			//
			// 		_mesh = mesh;
			// 		_mesh.SetActive(true);
			//
			// 		Transform.Attach(_mesh);
			// 		Animation.Init(_mesh.GetComponent<Animator>());
			//
			// 		if (StateMachine.IsState(PetStateType.Idle))
			// 		{
			// 			Animation.Idle();
			// 		}
			// 		else
			// 		{
			// 			Animation.Move();
			// 		}
			// 	}
			// }

			// SetColors(data.Level);
			// base.SetData(data);
		}

		public void SetColors(int level)
		{
			// var config = FactoryCollection.Get<CollectionConfig>();
			// var configColor = config.Get<ModelConfigParamEntityColor>();
			// SetColors(configColor.GetColors((level - 1) % 60));
		}

		private void SetColors(List<Color> colors)
		{
			var renderer = _mesh.GetComponentInChildren<SkinnedMeshRenderer>();
			var material = renderer.material;
			var keys = new List<string> { "_Color_Main", "_Color_R", "_Color_G", "_Color_B" };

			for (var i = 0; i < colors.Count; i++)
			{
				material.SetColor(keys[i], colors[i]);
			}
		}

	}
}