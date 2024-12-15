// Author:    -    ad
// Created: 18/07/2024  : : 11:14 PM
// DateUpdate: 18/07/2024

using Game.Runtime;
using UnityEngine;

namespace _Src.Scripts.Runtime.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class GameCamera : MonoBehaviour
    {
        void Start()
        {
            CameraManager.Instance.SetCamera(CameraManager.CameraType.GameCamera);
        }
    }
}