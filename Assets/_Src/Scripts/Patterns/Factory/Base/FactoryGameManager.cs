using Game.Runtime;
using Template.Defines;
using UnityEngine;

public class FactoryGameManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitAfterSceneLoad()
    {
        Instance.Init();
    }

    private static FactoryGameManager _instance;

    private static FactoryGameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = Instantiate(Resources.Load("Managers/FactoryGameManager") as GameObject);
            _instance = obj.GetComponent<FactoryGameManager>();
            DontDestroyOnLoad(obj);
            _instance.Init();
            return _instance;
        }
    }

    private void Init()
    {
        FactoryHelper.Setup();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            FactoryStorage.SaveAll();
        }
    }

    private void OnApplicationQuit()
    {
        FactoryStorage.SaveAll();
    }
}