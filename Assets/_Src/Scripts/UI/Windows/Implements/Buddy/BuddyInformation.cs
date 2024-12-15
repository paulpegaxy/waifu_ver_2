using UnityEngine;
using TMPro;
using Game.Model;

namespace Game.UI
{
	public class BuddyInformation : MonoBehaviour
	{
		[SerializeField] private TMP_Text _textLayer;
		[SerializeField] private TMP_Text _textIap;
		[SerializeField] private TMP_Text _textFriend;
		[SerializeField] private TMP_Text _textCommission;

		public void SetData(ModelApiFriendConfigLayer config)
		{
			_textLayer.text = $"{Localization.Get(TextId.Buddy_Layer)} {config.layer}";
			_textIap.text = $"{Localization.Get(TextId.Buddy_Spent)} ${config.iap}";
			_textFriend.text =
				$"{string.Format(Localization.Get(TextId.Buddy_HasFriend), config.invite, "L" + config.layer)}";
			_textCommission.text = $"{config.benefit}% {Localization.Get(TextId.Buddy_Commission)}";
		}
	}
}
