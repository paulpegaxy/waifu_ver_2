using System;
using System.Collections.Generic;
using BreakInfinity;
using Newtonsoft.Json;
using UnityEngine.Serialization;

[Serializable]
public class ModelApiLeaderboardRank
{
	public List<ModelApiLeaderboardData> leaderboard;
	public ModelApiLeaderboardData current_user_leaderboard;
	public int total_user;
	public int? global_rank;
}

[Serializable]
public class ModelApiLeaderboardData
{
	public string points;
	public string point;
	public float? boost;
	public string berry;
	
	[JsonProperty("rank_top")] public int rankPos;
	
	public string rank;
	
	public ModelApiLeaderboardUser user;
	
	public ModelApiLeaderboardClubDetail club;
	
	[JsonIgnore]
	public int Id => user?.id ?? club.id;
	
	[JsonIgnore]
	public string Name => user != null ? user.username : club.name;
	
	
	[JsonIgnore]
	public BigDouble PointsParse =>!string.IsNullOrEmpty(points)? BigDouble.Parse(points) : 0;

	[JsonIgnore]
	public TypeLeague RankParse => string.IsNullOrEmpty(rank)
		? TypeLeague.Bronze
		: (TypeLeague)Enum.Parse(typeof(TypeLeague), rank);
	
	public float Boost => boost ?? 1f;
}

[Serializable]
public class ModelApiLeaderboardUser
{
	public int id;
	public string username;
}

[Serializable]
public class ModelApiLeaderboardClubDetail
{
	public int id;
	public string name;
	public string total_members;
}