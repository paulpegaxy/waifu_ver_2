using System;
using UnityEngine;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

namespace Game.UI
{
	[RequireComponent(typeof(EnhancedScroller))]
	public class ESFlickSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] private EnhancedScroller.TweenType snapTweenType = EnhancedScroller.TweenType.easeOutSine;
		[SerializeField] private float snapTweenTime = 0.25f;

		public Action<int> OnJumpComplete;

		private EnhancedScroller _scroller;
		private Vector2 _dragStartPosition = Vector2.zero;
		private int _maxItems;
		private int _currentIndex = -1;
		private bool _isDragging = false;

		private void Awake()
		{
			_scroller = GetComponent<EnhancedScroller>();
		}

		public void SetMaxItems(int maxItems)
		{
			_maxItems = maxItems;
		}

		public void OnBeginDrag(PointerEventData data)
		{
			if (_scroller.IsTweening)
			{
				return;
			}

			if (_currentIndex == -1)
			{
				_currentIndex = _scroller.StartDataIndex;
			}

			_isDragging = true;
			_dragStartPosition = data.position;
		}

		public void OnEndDrag(PointerEventData data)
		{
			if (_isDragging)
			{
				_isDragging = false;

				var delta = data.position - _dragStartPosition;
				var jumpToIndex = -1;

				if (_scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal)
				{
					if (delta.x < 0)
					{
						if (_currentIndex < _maxItems - 1 || _scroller.Loop)
						{
							jumpToIndex = _currentIndex + 1;
						}
					}
					else if (delta.x > 0)
					{
						jumpToIndex = _currentIndex - 1;
					}
				}
				else
				{
					if (delta.y < 0)
					{
						jumpToIndex = _currentIndex - 1;
					}
					else if (delta.y > 0)
					{
						if (_currentIndex < _maxItems - 1 || _scroller.Loop)
						{
							jumpToIndex = _currentIndex + 1;
						}
					}
				}

				if (_scroller.Loop)
				{
					if (jumpToIndex < 0)
					{
						jumpToIndex += _maxItems;
					}
					else if (jumpToIndex >= _maxItems)
					{
						jumpToIndex -= _maxItems;
					}
				}

				if (jumpToIndex != -1)
				{
					_scroller.JumpToDataIndex(jumpToIndex, tweenType: snapTweenType, tweenTime: snapTweenTime, jumpComplete: () => OnJumpComplete?.Invoke(jumpToIndex));
					_currentIndex = jumpToIndex;
				}
			}
		}
	}
}
