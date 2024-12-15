using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class FriendBonusItem : MonoBehaviour
	{
		[SerializeField] private Image imgFrame;
		[SerializeField] private Image imageLeague;
		[SerializeField] private TMP_Text textNormalBonus;
		[SerializeField] private TMP_Text textPremiumBonus;

		public void SetData(ModelFriendConfigFriendBonus data, Color rowColor)
		{
			imgFrame.color = rowColor;
			// imageLeague.sprite = ControllerSprite.Instance.GetLeagueGirlIcon(data.league);
			imageLeague.LoadSpriteAutoParseAsync($"league_{(int)data.league}");
			// imageLeague.SetNativeSize();

			textNormalBonus.text = $"+{data.normal_friend}";
			textPremiumBonus.text = $"+{data.telegram_premium_friend}";
		}
	}
}