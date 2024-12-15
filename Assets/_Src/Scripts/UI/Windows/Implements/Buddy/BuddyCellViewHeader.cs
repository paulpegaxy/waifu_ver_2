using System;
using UnityEngine;
using Game.Model;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;

namespace Game.UI
{
	public class BuddyCellViewHeader : ESCellView<ModelBuddyCellView>
	{
		[SerializeField] private ItemCurrencyColor itemTonProcessing;
		[SerializeField] private ItemCurrencyColor itemTonBalance;
		[SerializeField] private TMP_Text textSpend;
		[SerializeField] private UIButton buttonWithdraw;
		
		public static Action<ModelBuddyCellViewHeader> OnClaim;
		
		private ModelBuddyCellViewHeader _data;

		private void OnEnable()
		{
			buttonWithdraw.onClickEvent.AddListener(OnWithdraw);
		}

		private void OnDisable()
		{
			buttonWithdraw.onClickEvent.RemoveListener(OnWithdraw);
		}

		private void OnWithdraw()
		{
			// // var popup = UIPopup.Get(UIId.UIPopupName.PopupWithdraw.ToString());
			// // popup.Show();
			
			
			// var apiFriend = FactoryApi.Get<ApiFriend>();
			// if (!apiFriend.Data.Config.IsAvailableToClaim())
			// {
			// 	ControllerPopup.ShowToastComingSoon();
			// 	return;
			// }
			
			if (_data.Total <= 0)
			{
				ControllerPopup.ShowToast(Localization.Get(TextId.Toast_NotiNothingWithdraw));
				return;
			}
			
			OnClaim?.Invoke(_data);
		}

		public override void SetData(ModelBuddyCellView model)
		{
			var data = model as ModelBuddyCellViewHeader;

			itemTonProcessing.SetAmount(data.Processing);
			itemTonBalance.SetAmount(data.Total);
		}
	}
}
