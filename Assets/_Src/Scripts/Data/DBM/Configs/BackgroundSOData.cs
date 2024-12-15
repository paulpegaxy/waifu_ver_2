using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public partial class Config
{
    [field: SerializeField] public BackgroundSOData backgroundConfig;
}
    
[CreateAssetMenu(fileName = "BackgroundSOData", menuName = "SO/Config/BackgroundSOData", order = 0)]
public class BackgroundSOData : ScriptableObject
{
    [field: SerializeField] private GenericDictionary<string, DataItemBackground> dictBackground;

    [field: SerializeField] private GenericDictionary<int, DataItemBackgroundCharNormal> dictBgNormalChar;
    
    public List<DataItemBackground> ListBgConfig => dictBackground.Values.ToList();

    public DataItemBackground GetBackground(string bgId)
    {
        if (dictBackground.TryGetValue(bgId, out DataItemBackground config))
            return config;
        
        return null;
    }
    
    public DataItemBackgroundCharNormal GetBackgroundCharNormalConfig(int charId)
    {
        if (dictBgNormalChar.TryGetValue(charId, out DataItemBackgroundCharNormal config))
        {
            return config;
        }

        return null;
    }
    
    public string GetBackgroundCharNormal(int charId,int level)
    {
        int modValue = (level + 1) % GameConsts.MAX_LEVEL_PER_CHAR;
        if (dictBgNormalChar.TryGetValue(charId, out DataItemBackgroundCharNormal config))
        {
            if (modValue == 0)
            {
                if (string.IsNullOrEmpty(config.bgLevel_10))
                    return $"{charId}_bg";

                return config.bgLevel_10;
            }

            if (modValue == GameConsts.MAX_LEVEL_PER_CHAR - 1)
            {
                if (string.IsNullOrEmpty(config.bgLevel_9))
                    return $"{charId}_bg";

                return config.bgLevel_9;
            }

            return $"{charId}_bg";
        }

        return $"{charId}_bg";
    }
    
    public DataItemBackground GetBackgroundByBgId(string bgId)
    {
        return dictBackground.Values.FirstOrDefault(x => x.backgroundId == bgId);
    }
        
    [Button("Save")]
    public void AutoProcessData()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}
    
[Serializable]
public class DataItemBackground
{
    public int id;
    public string backgroundId;
    public string name;
    public bool isHaveVfx;
    
    public bool IsYukiBackground()
    {
        return backgroundId.Equals("tokyo_night");
    }
}

[Serializable]
public class DataItemBackgroundCharNormal
{
    public string bgLevel_9;
    public string bgLevel_10;
}