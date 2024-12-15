using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Game.Core;

namespace Game.Runtime
{
	public class CameraManager : Singleton<CameraManager>
	{
		public enum CameraType
		{
			MainCamera,
			UICamera,
			GameCamera,
		}

		[ShowInInspector]
		private readonly Dictionary<CameraType, Camera> _cameras = new();

		void Start()
		{
			AddCamera(CameraType.UICamera);
			SetCamera(CameraType.MainCamera);
		}

		void AddCamera(CameraType cameraType)
		{
			GameObject cameraObject = GameObject.FindGameObjectWithTag(cameraType.ToString());
			if (cameraObject != null)
			{
				Camera camera = cameraObject.GetComponent<Camera>();
				if (_cameras.ContainsKey(cameraType))
				{
					_cameras[cameraType] = camera;
				}
				else
				{
					_cameras.Add(cameraType, camera);
				}
			}
		}

		public void RemoveCamera(CameraType cameraType)
		{
			if (_cameras.ContainsKey(cameraType))
			{
				_cameras.Remove(cameraType);
			}
		}

		public void SetCamera(CameraType cameraType)
		{
			if (!_cameras.ContainsKey(cameraType))
			{
				AddCamera(cameraType);
			}

			if (cameraType != CameraType.UICamera)
			{
				// UniversalAdditionalCameraData camData = _cameras[cameraType].GetUniversalAdditionalCameraData();
				// if (camData.cameraStack.Count == 0)
				// {
				// 	camData.cameraStack.Add(_cameras[CameraType.UICamera]);
				// }
			}

			// foreach (var camera in _cameras)
			// {
			// 	camera.Value.gameObject.SetActive(camera.Key == cameraType || camera.Key == CameraType.UICamera);
			// }
		}

		public Camera GetCamera(CameraType cameraType)
		{
			if (_cameras.ContainsKey(cameraType))
			{
				return _cameras[cameraType];
			}

			return null;
		}

		public void ShowCamera(CameraType cameraType)
		{
			if (_cameras.ContainsKey(cameraType))
			{
				_cameras[cameraType].enabled = true;
			}
		}

		public void HideCamera(CameraType cameraType)
		{
			if (_cameras.ContainsKey(cameraType))
			{
				_cameras[cameraType].enabled = false;
			}
		}

		public Vector3 WorldToScreenPoint(Vector3 worldPoint)
		{
			return _cameras[CameraType.UICamera].WorldToScreenPoint(worldPoint);
		}
	}
}