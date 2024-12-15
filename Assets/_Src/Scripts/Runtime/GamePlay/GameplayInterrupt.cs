using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Template.Defines;
using UnityEngine;

namespace Game.Runtime
{
	public class GameplayInterrupt : MonoBehaviour
	{
		public static Action<bool> OnVisibilityChanged;

#if UNITY_WEBGL
		[DllImport("__Internal")]
		private static extern string _RegisterVisibilityChangeEvent();
#endif

		private void Start()
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			_RegisterVisibilityChangeEvent();
#endif
		}

		private void OnVisibilityChange(string visibility)
		{
			bool isVisible = visibility == "visible";
			OnVisibilityChanged?.Invoke(isVisible);

			if (isVisible)
			{
				_ = ServiceTime.Refresh();
			}
			
			ServiceLocator.GetService<IServiceTracking>().UpdateExitTime();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				// FactoryApi.Get<ApiTracking>().PostTracking().Forget();
			}
		}
#endif
	}
}