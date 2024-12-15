using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using Game.Model;
using Game.Defines;
using Template.Defines;

namespace Game.UI
{
	public class JackpotWindow : UIWindow
	{
		[SerializeField] private JackpotScroller scroller;
		[SerializeField] private UIButton btnBuyTicket;
		[SerializeField] private UIButton buttonInfo;
		[SerializeField] private UIButton btnHistory;
		[SerializeField] private GameObject objDoneBuy;
		[SerializeField] private JackpotItemMyTickets itemMyTickets;

		private ModelApiQuestData _quest;

		protected override void OnEnabled()
		{
			btnBuyTicket.onClickEvent.AddListener(OnBuyTicket);
			buttonInfo.onClickEvent.AddListener(OnInfo);
			btnHistory.onClickEvent.AddListener(OnSeeHistory);

			// CheckQuest();
			Fetch();
		}

		private void OnDisable()
		{
			btnBuyTicket.onClickEvent.RemoveListener(OnBuyTicket);
			buttonInfo.onClickEvent.RemoveListener(OnInfo);
			btnHistory.onClickEvent.RemoveListener(OnSeeHistory);
		}

		private async void OnBuyTicket()
		{
			Signal.Send(StreamId.UI.OpenQuest);

			// this.ShowProcessing();
			// try
			// {
			// 	var apiShop = FactoryApi.Get<ApiShop>();
			// 	await apiShop.BuyWithTon(TypeShopItem.CheckIn);
			//
			// 	_quest.claimed = true;
			// 	_quest.can_claim = false;
			//
			// 	// CheckQuest();
			// 	Fetch();
			// 	this.HideProcessing();
			// }
			// catch (Exception e)
			// {
			// 	e.ShowError();
			// }
			// ControllerPopup.SetApiLoading(false);
		}

		private void OnInfo()
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupJackpot.ToString());
			popup.GetComponent<PopupJackpot>().SetData(FactoryApi.Get<ApiEvent>().Data.Jackpot, false);
			popup.Show();
		}

		// private void CheckQuest()
		// {
		// 	var apiQuest = FactoryApi.Get<ApiQuest>();
		// 	if (apiQuest.Data.Quest == null) return;
		//
		// 	_quest = apiQuest.Data.Quest.Find(x => x.Type == QuestType.DailyCheckIn);
		// 	if (_quest == null) return;
		//
		// 	btnBuyTicket.gameObject.SetActive(!_quest.can_claim && !_quest.claimed);
		// 	objDoneBuy.SetActive(!buttonCheckin.gameObject.activeSelf);
		// }

		private async void Fetch()
		{
			this.ShowProcessing();
			try
			{
				var apiEvent = FactoryApi.Get<ApiEvent>();
				var jackpot = await apiEvent.Jackpot();

				var data = new List<AModelJackpotCellView>()
				{
					new ModelJackpotCellViewHeader() { Jackpot = jackpot },
				};

				foreach (var history in jackpot.win_history)
				{
					data.Add(new ModelJackpotCellViewContent() { History = history });
				}

				itemMyTickets.SetData(jackpot.my_today_tickets);
				RecheckButton(jackpot.my_today_tickets);

				scroller.SetData(data);
				this.HideProcessing();	
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private void RecheckButton(List<int> listTicket)
		{
			bool isMaxBuy = listTicket.Count >= GameConsts.MAX_TICKET;

			// var apiQuest = FactoryApi.Get<ApiQuest>();
			// var listQuest = apiQuest.Data.Quest.FindAll(x => x.IsJackpotQuest());

			// bool isStillNotDoneQuest = listQuest.Exists(x => !x.can_claim && !x.claimed);

			objDoneBuy.SetActive(isMaxBuy);
			btnBuyTicket.gameObject.SetActive(!isMaxBuy);
			btnHistory.gameObject.SetActive(FactoryApi.Get<ApiEvent>().Data.JackpotHistories.Count > 0);

			// itemMyTickets.gameObject.SetActive(listTicket.Count > 0);
		}
		
		private void OnSeeHistory()
		{
			this.ShowPopup(UIId.UIPopupName.PopupJackpotMyHistory);
		}
	}
}