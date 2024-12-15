using System;
using UnityEngine;
using Template.Defines;

namespace Game.Runtime
{
	public class TutorialData : MonoBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] private TutorialObject Type;
		[HideInInspector] public string ObjectType;

		public TutorialObject TypeData => Type;

		public void SetData(TutorialObject type)
		{
			this.Type = type;
		}

		public void OnBeforeSerialize()
		{
			ObjectType = Type.ToString();
		}

		public void OnAfterDeserialize()
		{
			Type = (TutorialObject)Enum.Parse(typeof(TutorialObject), ObjectType);
		}
	}
}