using System;
using System.Collections.Generic;
using UnityEngine.Events;

public static class Observer<T> where T : Enum
{
    // private static Dictionary<T, UnityEvent<object>> events = new();

    private static readonly Dictionary<T, object> eventData = new();
    
    private static Dictionary<T, Action<object>> events = new();

    public static void RegisterListener(T id, Action<object> callback)
    {
        if (events.ContainsKey(id))
        {
            events[id] += callback;
        }
        else
        {
            events.Add(id, callback);
        }
    }
    public static void RemoveListener(T id, Action<object> callback)
    {
        if (events.ContainsKey(id))
        {
            events[id] -= callback;
        }
    }

    public static void PostEvent(T id, object param = null)
    {
        if (eventData.ContainsKey(id))
        {
            eventData[id] = param;
        }
        else
        {
            eventData.Add(id, param);
        }
        
        if (events.ContainsKey(id))
        {
            events[id]?.Invoke(param);
        }
    }

    public static TData GetData<TData>(T id, bool isRemove = false)
    {
        if (eventData.TryGetValue(id, out object data))
        {
            if (isRemove)
            {
                eventData.Remove(id);
            }
            return (TData)data;
        }
        return default;
    }
}