// Author: ad   -
// Created: 30/11/2024  : : 23:11
// DateUpdate: 30/11/2024

using Game.Runtime;
using UnityEngine;

namespace Template.Runtime
{
    public class ControllerDragDropNew : MonoBehaviour
    {
        private Camera Camera;
        protected bool IsDragging;
        protected Vector3 StartPosition;

        
        private void Update()
        {
            Camera = CameraManager.Instance.GetCamera(CameraManager.CameraType.UICamera);
            if (IsTouchDown())
            {
                if (ControllerPopup.IsAnyPopupVisible()) 
                    return;
		        
                Collider2D hitCollider = GetColliderInteract();
                if (hitCollider != null)
                {
                    OnStartDrag(hitCollider);
                }
            }
            else if (IsTouchDrag())
            {
                if (!IsDragging) 
                    return;

                OnDragging();
            }
            else if (IsTouchUp())
            {
                if (IsDragging)
                {
                    OnEndDragProcess();
                    IsDragging = false;
                }
            }
        }

        private Collider2D GetColliderInteract()
        {
            var camera = CameraManager.Instance.GetCamera(CameraManager.CameraType.MainCamera);
            Vector3 worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            StartPosition = worldPoint;
            Vector2 worldPoint2D = new Vector2(worldPoint.x, worldPoint.y);

// Check if the touch is over a UI element
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return null;
            }

            Collider2D hitCollider = Physics2D.OverlapPoint(worldPoint2D);

            return hitCollider;
        }

        protected void OnStartDrag(Collider2D hitCollider)
        {
            if (hitCollider.gameObject.tag.Equals("BannerGirl"))
            {
                IsDragging = true;
                Debug.LogError("hitCollider: " + hitCollider.name);
            }

        }

        protected void OnDragging()
        {
            
        }

        protected void OnEndDragProcess()
        {
            
        }

        private bool IsTouchDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        private bool IsTouchUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        private bool IsTouchDrag()
        {
            return Input.GetMouseButton(0);
        }

        protected Vector2 GetMousePosition()
        {
            return Input.mousePosition;
        }
    }
}