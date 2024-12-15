using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoInitAfterSceneLoad<T> : MonoBehaviour where T : MonoBehaviour
{
    private const string DEFAULT_FOLDER_LOAD_PATH = "Managers/";
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitAfterSceneLoad()
    {
#if !UNITY_SERVER || UNITY_EDITOR
        Debug.Log(Instance);
#endif
    }

    private static T _instance;

    public static T Instance
    {
        get
        {
            try
            {
                if (_instance != null) 
                    return _instance;
                
                _instance = (T)FindObjectOfType(typeof(T));
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    return _instance;
                }
                
                var objGet = Resources.Load($"Managers/{typeof(T).Name}") as GameObject;
                if (objGet == null)
                    throw new Exception();
                
                var singleton = Instantiate(objGet);
                if (singleton==null)
                    throw new Exception();
                
                _instance = singleton.GetComponent<T>();
                singleton.name = "(singleton_auto_init) " + typeof(T).Name;

                // _instance.Init();
                DontDestroyOnLoad(singleton);
            }
            catch (Exception e)
            {
                Debug.LogError($"Can not load {nameof(T)} in to Scene");
            }
            
            return _instance;
        }
    }

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        
    }
}
