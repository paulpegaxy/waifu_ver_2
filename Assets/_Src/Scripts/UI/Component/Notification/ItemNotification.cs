using System;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
	public abstract class ItemNotification : MonoBehaviour
	{
		protected abstract bool IsValid();

		private GameObject _objectIcon;

		private GameObject ObjIcon => transform.GetChild(0).gameObject;
		
		private bool _isInitiated;

		protected virtual void Awake()
		{
			ObjIcon.SetActive(false);
		}

		private void OnEnable()
		{
			if (!_isInitiated)
			{
				_isInitiated = true;
				return;
			}
			
			OnEnabled();

		}

		private void OnDisable()
		{
			OnDisabled();
		}

		protected virtual void OnEnabled()
		{
		}

		protected virtual void OnDisabled()
		{
			
		}

		protected void Refresh()
		{
			if (FactoryApi.Get<ApiGame>().Data.Info == null)
				return;
			if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.GameFeature))
			{
				
				if (!TutorialMgr.Instance.IsUnlockTutorial(TutorialCategory.GameFeature))
					return;
			}
			ObjIcon.SetActive(IsValid());
		}
	}

	public abstract class ItemNotificationGeneric<T> : ItemNotification
	{
		protected T _data;

		protected void SetData(T data)
		{
			_data = data;
			Refresh();
		}
	}
}