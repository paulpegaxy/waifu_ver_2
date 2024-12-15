using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;
using Game.Runtime;
using Game.Ton;
using Game.Defines;

namespace Game.UI
{
	public class PartnerCellViewCheckin : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private TMP_Text textAvailable;
		[SerializeField] private TMP_Text textClaimed;
		[SerializeField] private TMP_Text textRemaining;
		[SerializeField] private UIButton buttonClaim;
		[SerializeField] private GameObject objectClaimInactive;
		[SerializeField] private GameObject objectClaimed;
		[SerializeField] private GameObject objectAvailable;
		[SerializeField] private GameObject objectUnavailable;
		[SerializeField] private List<PartnerItemCheckin> items;

		private ModelPartnerCellViewCheckin _data;

		private void OnEnable()
		{
			buttonClaim.onClickEvent.AddListener(OnClaim);
		}

		private void OnDisable()
		{
			buttonClaim.onClickEvent.RemoveListener(OnClaim);
		}

		private async void OnClaim()
		{
			if (_data == null) return;

			ControllerPopup.SetApiLoading(true);
			try
			{
				if (!TONConnect.IsConnected) await TONConnect.ConnectWalletAsync();

				var apiEvent = FactoryApi.Get<ApiEvent>();
				var config = _data.Config;
				var claimData = await apiEvent.CheckInClaimSign(config.id, TONConnect.Wallet.account.userFriendlyAddress);
				var hash = await TonApi.Claim(claimData);

				if (hash == null)
				{
					ControllerPopup.ShowToast(Localization.Get(TextId.Toast_NotiClaimFailed));
				}
				else
				{
					await apiEvent.CheckInClaimComplete(config.id, claimData.id, hash);
					await apiEvent.CheckIn(config.id);

					SetData(_data);
					ControllerPopup.ShowToast(Localization.Get(TextId.Toast_NotiClaimSuccess));
				}
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);
		}

		public override void SetData(ModelPartnerCellView model)
		{
			var apiEvent = FactoryApi.Get<ApiEvent>();
			var data = model as ModelPartnerCellViewCheckin;

			var checkin = apiEvent.Data.GetCheckin(data.Config.id);
			if (checkin == null) return;

			for (int i = 0; i < items.Count; i++)
			{
				var status = i < checkin.days ? PartnerCheckinStatus.Claimed : PartnerCheckinStatus.Available;
				items[i].SetData(i + 1, status);
			}

			buttonClaim.gameObject.SetActive(checkin.can_claim && !checkin.is_claimed);

			textAvailable.text = string.Format(Localization.Get(TextId.Partner_Checkin1), checkin.total_users_can_claim, checkin.ton_reward_amount);
			textClaimed.text = checkin.total_users_claimed.ToString();
			textRemaining.text = (checkin.total_users_can_claim - checkin.total_users_claimed).ToString();

			objectAvailable.SetActive(checkin.total_users_claimed < checkin.total_users_can_claim);
			objectUnavailable.SetActive(checkin.total_users_claimed >= checkin.total_users_can_claim);
			objectClaimed.SetActive(checkin.is_claimed);
			objectClaimInactive.SetActive(!checkin.can_claim && !checkin.is_claimed);

			_data = data;
		}
	}
}