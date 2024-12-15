using System;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

namespace Game.UI
{
	public class ESMulti<TType, TModel, TCellView> : MonoBehaviour, IEnhancedScrollerDelegate where TType : Enum
	{
		[SerializeField] public EnhancedScroller scroller;
		[SerializeField] private List<EnhancedScrollerCellView> cellViews;

		protected List<TModel> _data;
		protected List<Vector2> _cellSize;

		private void UpdateCellSize()
		{
			_cellSize = new List<Vector2>();
			foreach (var cellView in cellViews)
			{
				_cellSize.Add(cellView.GetComponent<RectTransform>().sizeDelta);
			}
		}

		public void ReloadData()
		{
			if (scroller.ScrollSize == 0)
			{
				scroller.ReloadData();
			}
			else
			{
				scroller.ReloadData(scroller.ScrollPosition / scroller.ScrollSize);
			}
		}

		public void JumpToDataIndex(int index, EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.immediate, float tweenTime = 0)
		{
			scroller.JumpToDataIndex(index, tweenType: tweenType, tweenTime: tweenTime);
			ReloadData();
		}
		
		public void JumpTo(int index, EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.immediate, float tweenTime = 0)
		{
			scroller.JumpToDataIndex(index, tweenType: tweenType, tweenTime: tweenTime);
			ReloadData();
		}

		public int GetNumberOfCells(EnhancedScroller scroller)
		{
			return GetData().Count;
		}

		public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			var iData = _data[dataIndex] as IESModel<TType>;
			var size = _cellSize[Convert.ToInt32(iData.Type)];

			if (scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal)
			{
				return size.x;
			}
			
			return size.y;
		}

		public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
		{
			var iData = _data[dataIndex] as IESModel<TType>;
			var cellView = scroller.GetCellView(cellViews[Convert.ToInt32(iData.Type)]);
			cellView.name = "Cell " + dataIndex;

			var data = GetData();
			var item = cellView.GetComponent<ESCellView<TModel>>();
			item.SetData(data[dataIndex]);

			return cellView;
		}

		public virtual void SetData(List<TModel> data)
		{
			scroller.Delegate = this;
	
			UpdateCellSize();
			_data = data;
			
			OnSetData();
			ReloadData();
		}
		
		public virtual List<TModel> GetData()
		{
			return _data;
		}

		public virtual void OnSetData()
		{
		}
	}
}