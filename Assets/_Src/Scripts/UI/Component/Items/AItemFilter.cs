using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Cysharp.Threading.Tasks;


namespace Game.UI
{
	public abstract class AItemFilter<T> : MonoBehaviour where T : Enum
	{
		public static Action<T> OnChanged;

		protected UIToggleGroup _toggleGroup;

		private void Awake()
		{
			_toggleGroup = GetComponent<UIToggleGroup>();
		}

		private async void OnEnable()
		{
			await UniTask.DelayFrame(1);

			for (var i = 0; i < _toggleGroup.toggles.Count; i++)
			{
				var toggle = _toggleGroup.toggles[i];
				var index = i;

				toggle.onClickEvent.AddListener(() => OnSelect(index));
			}
		}

		private void OnDisable()
		{
			for (var i = 0; i < _toggleGroup.toggles.Count; i++)
			{
				var toggle = _toggleGroup.toggles[i];
				toggle.onClickEvent.RemoveAllListeners();
			}
		}

		private void OnSelect(int index)
		{
			var value = (T)Enum.ToObject(typeof(T), index);
			OnChanged?.Invoke(value);
		}

		public void SetData(T type)
		{
			var toggle = _toggleGroup.toggles[Convert.ToInt32(type)];
			if (toggle.isOn) return;

			toggle.SetIsOn(true);
		}
	}

}
