using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Model;
using Doozy.Runtime.UIManager.Components;
using System;
using Template.Defines;

namespace Game.UI
{
	public class AchievementCellView : ESCellView<ModelApiQuestData>
	{
		[SerializeField] private Image imageBackground;
		[SerializeField] private Image imageIcon;
		[SerializeField] private Image imageFriend;
		[SerializeField] private TMP_Text textBonus;
		[SerializeField] private TMP_Text textDescription;
		[SerializeField] private UIButton buttonClaim;
		[SerializeField] private GameObject objectHighlight;
		[SerializeField] private GameObject objectClaimed;
		[SerializeField] private List<Color> colorDescription;
		[SerializeField] private List<Sprite> iconFriend;

		public static Action<ModelApiQuestData, Vector3> OnClaim;

		private ModelApiQuestData _data;

		private void OnEnable()
		{
			buttonClaim.onClickEvent.AddListener(OnClaimClick);
		}

		private void OnDisable()
		{
			buttonClaim.onClickEvent.RemoveListener(OnClaimClick);
		}

		private void OnClaimClick()
		{
			OnClaim?.Invoke(_data, transform.position);
		}

		public override void SetData(ModelApiQuestData data)
		{
			var index = data.Category - QuestCategory.Checkin;

			// UnityEngine.Debug.LogError("index: " + data.achievement_index);

			// imageBackground.sprite = ControllerSprite.Instance.GetAchievementBg(index + 1);
			imageBackground.LoadSpriteAutoParseAsync("achievement_bg_" + (index + 1));

			var friend = iconFriend[index];
			if (friend != null)
			{
				imageFriend.sprite = friend;
			}
			imageFriend.gameObject.SetActive(friend != null);

			textBonus.text = $"+{data.items[0].quantity}";
			textDescription.text = data.description;
			textDescription.color = colorDescription[index];

			buttonClaim.interactable = data.can_claim;
			objectHighlight.SetActive(!data.claimed && data.can_claim);
			objectClaimed.SetActive(data.claimed);

			_data = data;
		}
	}
}
