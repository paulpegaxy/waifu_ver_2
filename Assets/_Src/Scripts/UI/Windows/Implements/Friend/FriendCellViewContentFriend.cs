using UnityEngine;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class FriendCellViewContentFriend : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private TMP_Text textName;
		[SerializeField] private ItemAvatar itemAvatar;
		[SerializeField] private TMP_Text textFriendCount;
		[SerializeField] private TMP_Text textScore;

		public override void SetData(ModelFriendCellView model)
		{
			var data = model as ModelFriendCellViewContentFriend;

			textName.text = data.Name.Truncate(GameConsts.MAX_LENGTH_NICK);
			itemAvatar.SetNameAvatar(data.Name);
			textFriendCount.text = $"+{data.FriendCount} {Localization.Get(TextId.Common_Friends)}";
			textScore.text = $"+{data.Score}";
		}
	}
}
