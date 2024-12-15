using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;

namespace Game.UI
{
	public class FriendDetailWindow : UIWindow
	{
		[SerializeField] private FriendDetail friendDetail;
		[SerializeField] private FriendBonus friendBonus;

		protected override void OnEnabled()
		{
			Refresh();
		}

		protected override void OnDisabled()
		{

		}

		private void Refresh()
		{
			var apiFriend = FactoryApi.Get<ApiFriend>();
			var config = apiFriend.Data.Config;
			var inviteBonus = config.referral_invite_bonus_config[0];

			friendDetail.SetData(inviteBonus.normal_friend, inviteBonus.telegram_premium_friend);
			friendBonus.SetData(config.referral_friend_bonus_config);
		}
	}
}