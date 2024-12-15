using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using BreakInfinity;
using Newtonsoft.Json;
using UnityEngine.Serialization;

[Serializable]
public class ModelApiLeaderboardConfig
{
    public List<ModelApiLeaderboardConfigData> rank_config;

    [JsonIgnore] public Dictionary<TypeLeague, ModelApiLeaderboardConfigData> DictConfigClub;

    [JsonIgnore] public List<ModelApiLeaderboardConfigData> ConfigPersonal;

    public void GetConfigClub(TypeLeague league, out ModelApiLeaderboardConfigData data)
    {
        Contract.Requires(DictConfigClub != null);
        DictConfigClub.TryGetValue(league, out data);
    }
    
    public void AutoParseData()
    {
        DictConfigClub = new Dictionary<TypeLeague, ModelApiLeaderboardConfigData>();
        ConfigPersonal = new List<ModelApiLeaderboardConfigData>();
        foreach (var data in rank_config)
        {
            if (data.type == FilterType.Club)
            {
                DictConfigClub.TryAdd((TypeLeague)data.league, data);
            }
            else
            {
                ConfigPersonal.Add(data);
            }
        }
    }
}


[Serializable]
public class ModelApiLeaderboardConfigData
{
    public int league;
    public FilterType type;
    public string from_point;
    public string to_point;
    
    public BigDouble ParseFromPoint()
    {
        return BigDouble.Parse(from_point);
    }
    
    public BigDouble ParseToPoint()
    {
        return to_point != "Infinity" ? BigDouble.Parse(to_point) : BigDouble.PositiveInfinity;
    }
}