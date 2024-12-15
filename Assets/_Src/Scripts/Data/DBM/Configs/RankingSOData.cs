
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Game.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public partial class Config
{
    [field: SerializeField] public RankingSOData rankingConfig;
}


[CreateAssetMenu(fileName = "RankingSOData", menuName = "SO/Config/RankingSOData", order = 0)]
public class RankingSOData : ScriptableObject
{
    [field: SerializeField] private GenericDictionary<TypeLeagueCharacter, DataItemRanking> dictRank;

    [field: SerializeField] private GenericDictionary<TypeLeague, DataItemClubRanking> dictRankClub;

    public List<DataItemRanking> GetListExistGirl()
    {
        return dictRank.Where(x => !string.IsNullOrEmpty(x.Value.girlName)).Select(x => x.Value).ToList();
    }

    public DataItemRanking GetRankData(TypeLeagueCharacter type)
    {
        if (dictRank.TryGetValue(type, out DataItemRanking config))
            return config;
        
        return null;
    }

    public DataItemClubRanking GetClubRankData(TypeLeague type)
    {
        if (dictRankClub.TryGetValue(type, out DataItemClubRanking config))
            return config;
        
        return null;
    }

    public DataItemRanking GetRankDataBasedGirlId(int girlID)
    {
        foreach (var item in dictRank)
        {
            if (item.Value.girlId == girlID)
                return item.Value;
        }

        return null;
    }
    
    public DataItemRanking GetDataBasedCurrentGirlLevel(int girlVisualLevel)
    {
        var currentRankIndex = (TypeLeagueCharacter) ((girlVisualLevel / GameConsts.MAX_LEVEL_PER_CHAR) + 1);
        var currentRank = GetRankData(currentRankIndex);
        return currentRank;
    }
    

    [Button("Auto Process Data")]
    public void AutoProcessData()
    {
#if UNITY_EDITOR
        ProcessData();
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    public void AutoLoadData(List<ModelApiLeaderboardConfigData> listData)
    {
#if UNITY_EDITOR
        int indexCount = 0;
        foreach (var item in dictRank)
        {
            if (item.Value.girlId <= 0)
            {
                break;
            }

            indexCount++;
        }
        
        for (var i = 0; i < indexCount; i++)
        {
            var ele = listData[i];
            
            var type = (TypeLeagueCharacter)listData[i].league;
            if (dictRank.TryGetValue(type, out DataItemRanking item))
            {
                item.fromValue = int.Parse(ele.from_point);
                item.toValue = int.Parse(ele.to_point);
                item.totalPointNextRank = item.toValue - item.fromValue;
            }
        }
        ProcessData();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    private void ProcessData()
    {
        foreach (var item in dictRank)
        {
            long stepPerLevel = item.Value.totalPointNextRank / GameConsts.MAX_LEVEL_PER_CHAR;
            item.Value.listPointVisualLevel = new List<long>();
            for (int i = 0; i < GameConsts.MAX_LEVEL_PER_CHAR; i++)
            {
                long value = item.Value.fromValue + (stepPerLevel * (i + 1));
                item.Value.listPointVisualLevel.Add(value);
            }
        }
    }
}

[Serializable]
public class DataItemRanking
{
    public TypeLeagueCharacter type;
    public long fromValue;
    public long toValue;
    public long totalPointNextRank;
    
    public int girlId;
    public string girlName;
    public Color rankColor;
    
    [ReadOnly] public List<long> listPointVisualLevel = new();
    
    public BigDouble GetPointNeedVisualLevel(int currentGirlLevel)
    {
        int level = currentGirlLevel % GameConsts.MAX_LEVEL_PER_CHAR;
        if (level >= listPointVisualLevel.Count)
            level = listPointVisualLevel.Count - 1;

        return BigDouble.Parse(listPointVisualLevel[level].ToString());
    }
}

[Serializable]
public class DataItemClubRanking
{
    public Color rankColor;
}