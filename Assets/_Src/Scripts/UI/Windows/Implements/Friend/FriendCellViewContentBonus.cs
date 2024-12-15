using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class FriendCellViewContentBonus : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private TMP_Text textNormalBonus;
		[SerializeField] private TMP_Text textPremiumBonus;
		[SerializeField] private UIButton buttonDetail;

		private void OnEnable()
		{
			buttonDetail.onClickEvent.AddListener(OnDetail);
		}

		private void OnDisable()
		{
			buttonDetail.onClickEvent.RemoveListener(OnDetail);
		}

		private void OnDetail()
		{

		}

		public override void SetData(ModelFriendCellView model)
		{
			var data = model as ModelFriendCellViewContentBonus;
			textNormalBonus.text = $"+{data.NormalBonus} {Localization.Get(TextId.Common_HcName)} {Localization.Get(TextId.Friend_ForYou)}";
			textPremiumBonus.text = $"+{data.PremiumBonus} {Localization.Get(TextId.Common_HcName)} {Localization.Get(TextId.Friend_ForYou)}";
		}
	}
}
