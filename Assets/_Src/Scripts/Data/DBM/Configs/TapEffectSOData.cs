
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public partial class Config
{
    [field: SerializeField] public TapEffectSOData tapEffectConfig;
}
    
[CreateAssetMenu(fileName = "TapEffectSOData", menuName = "SO/Config/TapEffectSOData", order = 0)]
public class TapEffectSOData : ScriptableObject
{
    [field: SerializeField] private GenericDictionary<string, DataItemTapEffect> dictTapEffect;
    
    public DataItemTapEffect tapSfwEffect;
    
    public DataItemTapEffect tapDefaultEffect => dictTapEffect["60000"];
    
    public List<DataItemTapEffect> ListTapEffConfig => dictTapEffect.Values.OrderBy(x=>x.id).ToList();

    public DataItemTapEffect GetTapEffect(string id)
    {
        if (dictTapEffect.TryGetValue(id, out DataItemTapEffect config))
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
public class DataItemTapEffect
{
    public int id;
    public string name;
    public string pathAssetName;
    public int resourceId;
    public bool isDefault;

    public string SlotTapAssetKey()
    {
        return $"slot_tap_{id}";
    }
}