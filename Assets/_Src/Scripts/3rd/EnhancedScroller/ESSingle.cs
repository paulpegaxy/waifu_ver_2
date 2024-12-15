using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

namespace Game.UI
{
	public class ESSingle<TModel, TCellView> : MonoBehaviour, IEnhancedScrollerDelegate
	{
		[SerializeField] public EnhancedScroller scroller;
		[SerializeField] private EnhancedScrollerCellView cellView;
		[SerializeField] private int ItemPerCell = 1;

		protected List<TModel> _data;
		protected Vector2 _cellSize;

		private void UpdateCellSize()
		{
			_cellSize = cellView.GetComponent<RectTransform>().sizeDelta;
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

		public int GetNumberOfCells(EnhancedScroller scroller)
		{
			return Mathf.CeilToInt((float)GetData().Count / ItemPerCell);
		}

		public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			if (scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal)
			{
				return _cellSize.x;
			}
			else
			{
				return _cellSize.y;
			}
		}

		public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
		{
			var cellView = scroller.GetCellView(this.cellView);
			if (ItemPerCell > 1)
			{
				cellView.name = "Cell " + (dataIndex * ItemPerCell) + " to " + ((dataIndex * ItemPerCell) + ItemPerCell - 1);
			}
			else
			{
				cellView.name = "Cell " + dataIndex;
			}

			var data = GetData();
			var item = cellView as ESCellView<TModel>;

			if (ItemPerCell > 1)
			{
				item.SetData(data, ItemPerCell * dataIndex);
			}
			else
			{
				item.SetData(data[dataIndex]);
			}

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