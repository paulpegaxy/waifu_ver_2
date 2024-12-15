#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LoadDataToConfigTool
{
    private const string TILE_OBJECT_CONFIG_PATH = "Assets/_Src/GameData/Configs/ScriptableTileObjectsConfig.asset";
    private const string HERO_OBJECT_DATA_FOLDER_PATH = "Assets/_Src/GameData/Hero";
    private const string MATERIAL_OBJECT_DATA_FOLDER_PATH = "Assets/_Src/GameData/Materials";


    [MenuItem("Game Tools/Reload Data")]
    public static void ReloadData()
    {
        ReloadTileObjectConfigPath();
    }

    private static void ReloadTileObjectConfigPath()
    {
        // ScriptableTileObjectsConfig tileConfig = AssetDatabase.LoadAssetAtPath<ScriptableObject>(TILE_OBJECT_CONFIG_PATH) as ScriptableTileObjectsConfig;
        // var allHeroConfig = LoadAllObjectInFolder(HERO_OBJECT_DATA_FOLDER_PATH).Select(obj => obj as ScriptableObjectData).ToList();
        // var allMaterialConfig = LoadAllObjectInFolder(MATERIAL_OBJECT_DATA_FOLDER_PATH).Select(obj => obj as ScriptableObjectData).ToList();

        // tileConfig.Clear();
        // foreach (var config in allMaterialConfig)
        // {
        //     if (config == null) { continue; }
        //     tileConfig.Add(config);
        // }
        // foreach (var config in allHeroConfig)
        // {
        //     if (config == null) { continue; }
        //     tileConfig.Add(config);
        // }

        // Debug.Log($"tile config {tileConfig?.name}");
        // EditorUtility.SetDirty(tileConfig);
        // AssetDatabase.SaveAssetIfDirty(tileConfig);
    }

    private static IEnumerable<ScriptableObject> LoadAllObjectInFolder(string folderPath)
    {
        string[] allFilePaths = Directory.GetFiles(folderPath);
        foreach (string filePath in allFilePaths)
        {
            yield return AssetDatabase.LoadAssetAtPath<ScriptableObject>(filePath);
        }

    }
}

#endif