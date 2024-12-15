using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class ShopPanelPeriod : MonoBehaviour
	{
		[SerializeField] private UIToggleGroup filterPeriod;
		[SerializeField] private ShopScrollerPeriod scroller;

		private enum PeriodType
		{
			Daily,
			Weekly,
		}

		private PeriodType _periodType;

		private void OnEnable()
		{
			filterPeriod.OnToggleTriggeredCallback.AddListener(OnTogglePeriod);
			ModelApiShop.OnChanged += OnDataChanged;
		}

		private void OnDisable()
		{
			filterPeriod.OnToggleTriggeredCallback.RemoveListener(OnTogglePeriod);
			ModelApiShop.OnChanged -= OnDataChanged;
		}

		private void OnTogglePeriod(UIToggle toggle)
		{
			_periodType = filterPeriod.lastToggleOnIndex == 0 ? PeriodType.Daily : PeriodType.Weekly;
			Refresh();
		}

		private void OnDataChanged(ModelApiShop data)
		{
			Refresh();
		}

		public void Refresh()
		{
			var apiShop = FactoryApi.Get<ApiShop>();
			if (_periodType == PeriodType.Daily)
			{
				scroller.SetData(apiShop.Data.GetDailyItems());
			}
			else
			{
				scroller.SetData(apiShop.Data.GetWeeklyItems());
			}
		}
	}
}