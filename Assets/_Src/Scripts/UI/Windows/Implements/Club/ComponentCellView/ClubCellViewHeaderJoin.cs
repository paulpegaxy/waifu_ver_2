using System;
using UnityEngine;
using Game.Model;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;

namespace Game.UI
{
	public class ClubCellViewHeaderJoin : ESCellView<ModelClubCellView>
	{
		[SerializeField] private ClubInformation information;
		[SerializeField] private UIButton buttonJoin;
		[SerializeField] private UIButton buttonBoost;

		public static Action<ModelApiClubData> OnBoostClub;
		public static Action<ModelApiClubData> OnJoinClub;

		private ModelClubCellViewHeaderJoin _data;

		private void OnEnable()
		{
			buttonJoin.onClickEvent.AddListener(OnJoin);
			buttonBoost.onClickEvent.AddListener(OnBoost);
		}

		private void OnDisable()
		{
			buttonJoin.onClickEvent.RemoveListener(OnJoin);
			buttonBoost.onClickEvent.RemoveListener(OnBoost);
		}

		private void OnJoin()
		{
			OnJoinClub?.Invoke(_data.Club);
		}

		private void OnBoost()
		{
			ControllerPopup.ShowToastComingSoon();
			return;
			
			OnBoostClub?.Invoke(_data.Club);
		}

		public override void SetData(ModelClubCellView model)
		{
			var data = model as ModelClubCellViewHeaderJoin;
			information.SetData(data.Club);

			var myInfo = FactoryApi.Get<ApiUser>().Data;
			buttonJoin.gameObject.SetActive(myInfo.Club == null);
			
			_data = data;
		}
	}
}
