using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using BreakInfinity;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
	public class PlayerWindow : UIWindow
	{
		[SerializeField] private TMP_Text lbTitle;
		[SerializeField] private TMP_Text textTotalGold;
		[SerializeField] private UIButton buttonMeaning;
		[SerializeField] private List<PlayerItem> playerItems;

		protected override void OnEnabled()
		{
			buttonMeaning.onClickEvent.AddListener(OnMeaning);

			Refresh(null);
			Fetch();
		}

		private void Start()
		{
			lbTitle.text = $"Total {Localization.Get(TextId.Common_ScName)} created";
		}

		protected override void OnDisabled()
		{
			buttonMeaning.onClickEvent.RemoveListener(OnMeaning);
		}

		private void OnMeaning()
		{
			// var popup = UIPopup.Get(UIId.UIPopupName.PopupGoldMeaning.ToString());
			// popup.Show();
		}

		private async void Fetch()
		{
			var apiCommon = FactoryApi.Get<ApiCommon>();
			var data = await apiCommon.GetSummary();

			Refresh(data);
		}

		private void Refresh(ModelApiCommonSummary data)
		{
			var titles = new List<TextId>()
			{
				TextId.Common_Players,
				TextId.Common_PremiumUsers,
				// TextId.Common_DailyUsers,
				TextId.Common_Online,
			};

			var numbers = new List<int>()
			{
				data != null ? data.total_user : 0,
				data != null ? data.total_premium_users : 0,
				// data != null ? data.totalLoginUsers : 0,
				data != null ? data.total_online_users : 0,
			};

			for (var i = 0; i < playerItems.Count; i++)
			{
				playerItems[i].SetData(Localization.Get(titles[i]), numbers[i], titles[i] == TextId.Common_Online);
			}

			if (data != null) textTotalGold.text = data.TotalPointCreatedParse.ToLetter();
		}
	}
}