using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    protected static T instance;

    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = Resources.Load(typeof(T).Name) as T;
        }
        return instance;
    }
}