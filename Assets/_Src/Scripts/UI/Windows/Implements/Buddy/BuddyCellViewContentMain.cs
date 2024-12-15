using Doozy.Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;
using Game.Model;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;

namespace Game.UI
{
	public class BuddyCellViewContentMain : ESCellView<ModelBuddyCellView>
	{
		[SerializeField] private BuddyReward buddyReward;
		[SerializeField] private BuddyProgress buddyProgress;
		[SerializeField] private TMP_Text textFriend;
		[SerializeField] private TMP_Text textSpend;
		[SerializeField] private TMP_Text textPrice;
		[SerializeField] private Image imageFriend;
		[SerializeField] private Sprite spriteFriend;
		[SerializeField] private Sprite spriteCheck;
		[SerializeField] private UIButton buttonUnlock;
		[SerializeField] private GameObject objectLock;
		[SerializeField] private GameObject objectUnlock;

		private ModelBuddyCellViewContentNormal _data;

		private void OnEnable()
		{
			buttonUnlock.onClickEvent.AddListener(OnUnlock);
		}

		private void OnDisable()
		{
			buttonUnlock.onClickEvent.RemoveListener(OnUnlock);
		}

		private void OnUnlock()
		{
			string des = string.Format(Localization.Get(TextId.Friend_NotiSpend), _data.UnlockPrice.ToDigit());
			ControllerPopup.ShowConfirm(des, Localization.Get(TextId.Common_GoTo), onOk:
				popup =>
				{
					popup.Hide();
					Signal.Send(StreamId.UI.OpenShop);
				});
		}

		public override void SetData(ModelBuddyCellView model)
		{
			var data = model as ModelBuddyCellViewContentNormal;
			_data = data;
			buddyReward.SetData(data);

			var progress = data.TotalSpend / data.MaxSpend;
			objectLock.SetActive(progress < 1);
			objectUnlock.SetActive(progress >= 1);
			buddyProgress.SetProgress(progress);

			if (data.CurrentFriend < data.MaxFriend)
			{
				textFriend.text = $"{Localization.Get(TextId.Buddy_UnlockFriend)} {data.CurrentFriend}<color=#8CC1FF>/{data.MaxFriend}</color>";
				imageFriend.sprite = spriteFriend;
			}
			else
			{
				textFriend.text = $"{Localization.Get(TextId.Buddy_UnlockFriend)} {data.CurrentFriend}/{data.MaxFriend}";
				imageFriend.sprite = spriteCheck;
			}
			imageFriend.SetNativeSize();

			var value = Mathf.Min(data.TotalSpend, data.MaxSpend);

			// textSpend.text = $"{Localization.Get(TextId.Buddy_Spend)} {value:.##}/${data.MaxSpend}";
			textSpend.text = $"{Localization.Get(TextId.Buddy_Spend)} ${Mathf.Min(data.TotalSpend, data.MaxSpend).ToDigit()}/${data.MaxSpend}";

			textPrice.text = $"${data.UnlockPrice:.##}";

			_data = data;
		}
	}
}
