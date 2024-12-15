#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using Game.Runtime;


public class EditorTools : Editor
{
    [MenuItem("Tools/Reset PlayerPrefs #&r", false)]
    public static async void ResetPlayerPref()
    {

        Debug.Log("*** PlayerPrefs was reset! ***");
#if !PRODUCTION_BUILD
                await FactoryApi.Get<ApiGame>().ResetData();
#endif


        PlayerPrefs.DeleteAll();
        // FactoryApi.CleanUp();
        FactoryStorage.Init();
        AnR.ReleaseAll();
    }
}

#endif