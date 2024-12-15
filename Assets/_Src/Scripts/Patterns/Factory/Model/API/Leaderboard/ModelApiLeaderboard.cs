using System;
using System.Collections.Generic;
using Game.Model;

[Serializable]
public class ModelApiLeaderboard : ModelApiNotification<ModelApiLeaderboard>
{
	public ModelApiLeaderboardConfig Config;
	public ModelApiLeaderboardRank LeaderboardBerry;

	public override void Notification()
	{
		OnChanged?.Invoke(this);
	}
}