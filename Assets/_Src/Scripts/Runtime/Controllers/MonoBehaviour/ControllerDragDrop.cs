using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Game.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace Template.Runtime
{
	public class ControllerDragDrop : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
	{
		[SerializeField] private float lengthDragRequire = 200;
		[SerializeField] private float xAxisRequire = 0.5f;

		public static Action<bool> OnEndDragWithTarget;
		
		private bool _isDragging;
		private Vector2 _startDragPosition;
		private Transform _currentTransform;
		
		// public void OnPointerClick(PointerEventData eventData)
		// {
		// 	if (eventData.pointerEnter != null && eventData.pointerEnter.gameObject.tag.Equals("BannerGirl"))
		// 	{
		// 		Debug.LogError("Hit to object " + eventData.pointerEnter.name);
		// 		_currentTransform = eventData.pointerEnter.transform;
		// 		_isDragging = true;
		// 		_startDragPosition = eventData.position;
		// 	}
		// }
		
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.pointerEnter != null && eventData.pointerEnter.gameObject.tag.Equals("BannerGirl"))
			{
				// Debug.LogError("Hit to object " + eventData.pointerEnter.name);
				// _currentTransform = eventData.pointerEnter.transform;
				_isDragging = true;
				_startDragPosition = eventData.position;
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
// #if !PRODUCTION_BUILD
			if (_isDragging)
			{
				_isDragging = false;
				var distance = Vector2.Distance(eventData.position, _startDragPosition);
				var value = eventData.position - _startDragPosition;
				var absX = Mathf.Abs(value.normalized.x);

				if (absX >= xAxisRequire && distance > lengthDragRequire)
				{
					// Debug.LogError(value.normalized.x);
					EndDragWithTarget(value.normalized.x < 0);
					// ShowDatingWindow(value.normalized.x > 0);
				}
			}
// #endif
		}

		private void EndDragWithTarget(bool isSwipeRightSide)
		{
			OnEndDragWithTarget?.Invoke(isSwipeRightSide);
		}
		

		private void ShowDatingWindow(bool isRightSide)
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirmDating.ToString());
			popup.GetComponent<PopupConfirmDating>().SetGirl(isRightSide);
			popup.Show();
		}
	}
}