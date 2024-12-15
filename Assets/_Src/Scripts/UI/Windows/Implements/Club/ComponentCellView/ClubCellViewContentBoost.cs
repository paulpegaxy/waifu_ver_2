using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class ClubCellViewContentBoost : ESCellView<ModelClubCellView>
	{
		[SerializeField] private Image imageRank;
		[SerializeField] private TMP_Text textRank;
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private TMP_Text textPrice;
		[SerializeField] private UIButton buttonSelect;
		[SerializeField] private GameObject objectHighlight;
		[SerializeField] private GameObject objectOverlay;

		public static Action<ModelClubCellViewContentBoost> OnSelect;

		private ModelClubCellViewContentBoost _data;

		private void OnEnable()
		{
			buttonSelect.onClickEvent.AddListener(OnSelectClick);
		}

		private void OnDisable()
		{
			buttonSelect.onClickEvent.RemoveListener(OnSelectClick);
		}

		private void OnSelectClick()
		{
			OnSelect?.Invoke(_data);
		}

		public override void SetData(ModelClubCellView model)
		{
			var data = model as ModelClubCellViewContentBoost;
			if (data == null)
				return;

			var rank = data.Rank;

			ExtensionImage.LoadRankIcon(imageRank, rank);

			textRank.text = rank.ToString();
			textTitle.text = $"{Localization.Get(TextId.Confirm_BoostTo)} {data.Info.rank}";
			textPrice.text = $"${data.Info.price}";

			objectHighlight.SetActive(data.IsSelected);
			objectOverlay.SetActive(!data.Info.available);

			buttonSelect.interactable = data.Info.available;

			_data = data;
		}
	}
}
