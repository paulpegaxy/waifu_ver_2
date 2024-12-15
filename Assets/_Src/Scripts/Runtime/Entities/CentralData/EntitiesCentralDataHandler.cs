using System;
using System.Collections.Generic;

public class EntitiesCentralDataHandler
{
    private Dictionary<Type, object> entityViewModels = new Dictionary<Type, object>();

    public void Reset()
    {
        entityViewModels.Clear();
    }
    public T GetOrCreate<T>()
    {
        Type key = typeof(T);
        if (!entityViewModels.ContainsKey(key))
        {
            entityViewModels.Add(key, (T)Activator.CreateInstance(key));
        }
        return (T)entityViewModels[key];
    }
}