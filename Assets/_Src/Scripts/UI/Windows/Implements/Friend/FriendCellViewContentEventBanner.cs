using UnityEngine;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class FriendCellViewContentEventBanner : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private ItemTimerAutoLabel itemTimer;

		public override void SetData(ModelFriendCellView model)
		{
			var data = model as ModelFriendCellViewContentEventBanner;
			var duration = data.Config.time_end - ServiceTime.CurrentUnixTime;

			itemTimer.SetDuration(duration);
		}
	}
}
