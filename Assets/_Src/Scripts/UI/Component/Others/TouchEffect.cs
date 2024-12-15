using System;
using System.Collections.Generic;
using Game.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
	public class TouchEffect : MonoBehaviour
	{
		[SerializeField] private Canvas canvas;

		// private GraphicRaycaster _raycaster;
		// private GraphicRaycaster Raycaster => _raycaster ??= GetComponent<GraphicRaycaster>();

		private Vector3 _startPosition;

		public static Action<object> OnClick;


		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				var effect = ControllerSpawner.Instance.Spawn(AnR.GetKey(AnR.CommonKey.VfxSelection), true, true, transform);
				if (effect == null) return;
		
				var rectTransform = effect.GetComponent<RectTransform>();
				rectTransform.localPosition =
					(Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / canvas.scaleFactor;
				effect.SetActive(true);
			}
		}

		private RaycastHit GetRaycastHit()
		{
			var camera = CameraManager.Instance.GetCamera(CameraManager.CameraType.UICamera);
			var ray = camera.ScreenPointToRay(Input.mousePosition);

			Physics.Raycast(ray, out var hit);

			return hit;
		}
	}
}