using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CharPremiumSOData", menuName = "SO/Config/CharPremiumSOData", order = 0)]
public class CharPremiumSOData : ScriptableObject
{
    [field: SerializeField] private GenericDictionary<int, DataItemCharPremium> dictChar;

    [field: SerializeField] private GenericDictionary<int, DataItemCharPremium> dictCharComingSoon;
    
    public List<DataItemCharPremium> ListCharConfig => dictChar.Values.ToList();

    public List<DataItemCharPremium> ListCharComingSoonConfig => dictCharComingSoon.Values.ToList();
    
    public DataItemCharPremium GetCharData(int charId)
    {
        if (dictChar.TryGetValue(charId, out DataItemCharPremium config))
            return config;
        
        return null;
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
public class DataItemCharPremium
{
    public int charId;
    public string name;
}

public partial class Config
{
    [field: SerializeField] public CharPremiumSOData charPremiumConfig;
}