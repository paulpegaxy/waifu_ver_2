using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private static readonly Dictionary<Type, object> serviceCaches = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        Type key = typeof(T);
        if (!serviceCaches.ContainsKey(key))
        {
            serviceCaches.Add(key, service);
        }
        else
        {
            serviceCaches[key] = service;
        }
    }

    public static T GetService<T>()
    {
        Type key = typeof(T);
        if (!serviceCaches.ContainsKey(key))
        {
            throw new ArgumentException(string.Format("Type '{0}' has not been registered.", key.Name));
        }
        return (T)serviceCaches[key];
    }
}