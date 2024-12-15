using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiFriend : ModelApiNotification<ModelApiFriend>
	{
		public ModelApiFriendConfig Config;
		public ModelApiFriendInvited Invited;
		public ModelApiFriendEventConfig EventConfig;
		public ModelApiFriendEventLeaderboard EventLeaderboard;

		public override void Notification()
		{
			OnChanged?.Invoke(this);
		}
	}
}