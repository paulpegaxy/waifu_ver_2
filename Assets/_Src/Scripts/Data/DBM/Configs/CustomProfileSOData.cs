
using System;
using System.Collections.Generic;
using _Src.Scripts.Data.DBM.Configs;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public partial class Config
{
    [field: SerializeField] public CustomProfileSOData customProfileConfig;
}

namespace _Src.Scripts.Data.DBM.Configs
{
    [CreateAssetMenu(fileName = "CustomProfileSOData", menuName = "SO/Config/CustomProfileSOData", order = 0)]
    public class CustomProfileSOData : ScriptableObject
    {
        [field: SerializeField] private List<DataItemStoryGenres> listStoryGenres;
        [field: SerializeField] private List<DataItemCustomProfileVisual> listVisual;

        public List<DataItemCustomProfileVisual> ListVisual => listVisual;
        
        public List<DataItemStoryGenres> ListStoryGenres => listStoryGenres;
        
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
    public class DataItemStoryGenres
    {
        public Sprite sprIcon;
        public string name;
    }

    [Serializable]
    public class DataItemCustomProfileVisual
    {
        public Sprite avatar;
    }
}