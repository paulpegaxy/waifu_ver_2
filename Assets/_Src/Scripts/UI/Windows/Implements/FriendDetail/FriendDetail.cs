using UnityEngine;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class FriendDetail : MonoBehaviour
	{
		[SerializeField] private TMP_Text textNormalBonus;
		[SerializeField] private TMP_Text textPremiumBonus;

		public void SetData(ModelFriendCellViewContentBonus data)
		{
			SetData(data.NormalBonus, data.PremiumBonus);
		}

		public void SetData(int normal, int premium)
		{
			textNormalBonus.text = $"+{normal} {Localization.Get(TextId.Common_HcName)} {Localization.Get(TextId.Friend_ForYou)}";
			textPremiumBonus.text = $"+{premium} {Localization.Get(TextId.Common_HcName)} {Localization.Get(TextId.Friend_ForYou)}";
		}
	}
}