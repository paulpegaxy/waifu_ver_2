// Author: 
// Created Date: 23/07/2024
// Update Time: 23/07

using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Template.Defines;
using UnityEngine;

public class GameDependencyManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitAfterSceneLoad()
    {
        Instance.Init();
    }

    private static GameDependencyManager _instance;

    public static GameDependencyManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = Instantiate(Resources.Load("Managers/GameDependencyManager") as GameObject);
            _instance = obj.GetComponent<GameDependencyManager>();
            DontDestroyOnLoad(obj);
            return _instance;
        }
    }
    

    public enum AtlasType
    {
        Icon,
        ResourceIcon,
    }

    private void Init()
    {
        GameDependencies.Initialize().Forget();
    }
}
