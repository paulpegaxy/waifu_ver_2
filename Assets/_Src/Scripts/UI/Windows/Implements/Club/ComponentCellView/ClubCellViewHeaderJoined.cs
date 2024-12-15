using UnityEngine;
using TMPro;
using Game.Model;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using System;

namespace Game.UI
{
	public class ClubCellViewHeaderJoined : ESCellView<ModelClubCellView>
	{
		[SerializeField] private ClubInformation information;
		[SerializeField] private UIButton buttonBoost;
		[SerializeField] private UIButton buttonLeave;

		public static Action<ModelApiClubData> OnBoostClub;
		public static Action<ModelApiClubData> OnLeaveClub;

		private ModelClubCellViewHeaderJoined _data;

		private void OnEnable()
		{
			buttonLeave.onClickEvent.AddListener(OnLeave);
			buttonBoost.onClickEvent.AddListener(OnBoost);
		}

		private void OnDisable()
		{
			buttonLeave.onClickEvent.RemoveListener(OnLeave);
			buttonBoost.onClickEvent.RemoveListener(OnBoost);
		}

		private void OnBoost()
		{
			OnBoostClub?.Invoke(_data.Club);
		}

		private void OnLeave()
		{
			ControllerPopup.ShowWarning(
				message: string.Format(Localization.Get(TextId.Confirm_LeaveClub), _data.Club.name),
				ok: Localization.Get(TextId.Confirm_Leave),
				onOk: popup =>
				{
					OnLeaveClub?.Invoke(_data.Club);
					popup.Hide();
				}
			);
		}

		public override void SetData(ModelClubCellView model)
		{
			var data = model as ModelClubCellViewHeaderJoined;
			information.SetData(data.Club);

			_data = data;
		}
	}
}
