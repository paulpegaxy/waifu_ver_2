using System;
using System.Collections;
using UnityEngine;

public static class ExtensionDelayMethod
{
    public static Coroutine StartDelayMethod(this MonoBehaviour mono, float time, Action callback)
    {
        return mono.StartCoroutine(Delay(time, callback));
    }

    private static IEnumerator Delay(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        if (callback != null)
        {
            callback.Invoke();
        }
        else
        {
            Debug.Log("Call back is destroyed");
        }
    }
}
