using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Cysharp.Threading.Tasks;


namespace Game.UI
{
	public abstract class TabSelector<T> : MonoBehaviour where T : Enum
	{
		public static Action<T> OnChanged;

		protected UIToggleGroup _toggleGroup;
		protected T _current;

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
			if (_current.Equals(value)) return;

			_current = value;
			OnChanged?.Invoke(value);
		}

		public void SetData(T type, bool notification = true)
		{
			var index = Convert.ToInt32(type);
			if (index < 0 || index >= _toggleGroup.toggles.Count) return;

			var toggle = _toggleGroup.toggles[index];
			if (toggle.isOn) return;

			toggle.SetIsOn(true);

			_current = type;

			if (notification)
			{
				OnChanged?.Invoke(type);
			}
		}
	}

}
