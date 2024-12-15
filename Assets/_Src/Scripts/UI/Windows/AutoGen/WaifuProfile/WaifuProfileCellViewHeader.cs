using System;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class WaifuProfileCellViewHeader : ESCellView<ModelWaifuProfileCellView>
	{
		[SerializeField] private UIButton btnUnmatch;
		[SerializeField] private ItemWaifuAvatar itemWaifuAvatar;
		[SerializeField] private ItemWaifuLevelProgress itemLevelProgress;
		[SerializeField] private TMP_Text txtCharName;
		[SerializeField] private TMP_Text txtLevel;

		private ModelApiEntityConfig _data;
		
		public override void SetData(ModelWaifuProfileCellView model)
		{
			if (model is ModelWaifuProfileCellViewHeader data)
			{
				_data = data.Data;
				txtCharName.text = _data.name;
				txtLevel.text = $"Level {_data.level}";
				itemWaifuAvatar.SetAvatar(_data);
				itemLevelProgress.SetData(_data);
			}
		}

		private void OnEnable()
		{
			btnUnmatch.onClickEvent.AddListener(OnClickUnMatch);
		}

		private void OnDisable()
		{
			btnUnmatch.onClickEvent.RemoveListener(OnClickUnMatch);
		}

		private async void OnClickUnMatch()
		{
			this.ShowProcessing();
			try
			{
				var apiEntity = FactoryApi.Get<ApiEntity>();
				await apiEntity.PostDeclineGirl(_data.id);
				await apiEntity.Get();
				ControllerPopup.ShowToastSuccess("Unmatch success");
				Signal.Send(StreamId.UI.BackToSwipe);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}
	}
}