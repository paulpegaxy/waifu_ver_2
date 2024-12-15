using System;
using System.Collections.Generic;
using BreakInfinity;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Serialization;

[Serializable]
public class ModelApiClubData
{
	public int id;
	public int total_members;
	[JsonProperty("rank")]
	public TypeLeague league;
	public string name;
	public string total_point;
	public string telegram_link;
	public int rank_top;
	// public List<ModelApiLeaderboardData> leaderboard_daily;
	// public List<ModelApiLeaderboardData> leaderboard_weekly;
	public List<ModelApiLeaderboardData> leaderboard_all_time;

	public BigDouble TotalPointParse => BigDouble.Parse(total_point);
}