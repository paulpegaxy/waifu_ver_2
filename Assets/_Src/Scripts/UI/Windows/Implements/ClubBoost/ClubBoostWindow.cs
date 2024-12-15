using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Core;
using Game.Extensions;
using Game.Runtime;
using Game.Model;
using Template.Defines;
using Template.Patterns;

namespace Game.UI
{
	public class ClubBoostWindow : UIWindow
	{
		[SerializeField] private ClubScroller _scrollerMain;
		[SerializeField] private UIButton buttonBoost;

		private ModelApiClubData _club;
		private ModelClubCellViewContentBoost _boostItem;

		protected override void OnEnabled()
		{
			ClubCellViewContentBoost.OnSelect += OnSelect;
			buttonBoost.onClickEvent.AddListener(OnBoost);

			var club = this.GetEventData<TypeGameEvent,ModelApiClubData>(TypeGameEvent.ClubBoost);
			Fetch(club);
		}

		protected override void OnDisabled()
		{
			ClubCellViewContentBoost.OnSelect -= OnSelect;
			buttonBoost.onClickEvent.RemoveListener(OnBoost);
		}

		private void OnSelect(ModelClubCellViewContentBoost selectItem)
		{
			var data = _scrollerMain.GetData();
			foreach (var item in data)
			{
				if (item is ModelClubCellViewContentBoost content)
				{
					content.IsSelected = content == selectItem;
				}
			}

			_boostItem = selectItem;
			_scrollerMain.SetData(data);
		}

		private async void OnBoost()
		{
			if (_boostItem == null) return;

			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiShop = FactoryApi.Get<ApiShop>();
				// var data = await apiShop.BoostWithTon(_club.id, _boostItem.Info.price);

				ControllerPopup.ShowToast(Localization.Get(TextId.Toast_ClubBoostSucess));
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);

		}

		private async void Fetch(ModelApiClubData club)
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiClub = FactoryApi.Get<ApiClub>();
				var prices = await apiClub.GetBoostPrices(club.id);
				var data = new List<ModelClubCellView>
				{
					new ModelClubCellViewHeaderBoost()
				};

				for (var i = 0; i < prices.Count; i++)
				{
					data.Add(new ModelClubCellViewContentBoost()
					{
						Rank = i + 1,
						IsSelected = i == 0,
						Info = prices[i]
					});
				}

				_boostItem = data[1] as ModelClubCellViewContentBoost;
				_scrollerMain.SetData(data);
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);

			_club = club;
		}
	}
}