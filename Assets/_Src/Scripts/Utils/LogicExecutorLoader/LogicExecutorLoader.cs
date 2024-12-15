using System;
using System.Collections.Generic;
using System.Reflection;
using Debug = UnityEngine.Debug;

public class LogicExecutorLoader<TKey> where TKey : Enum
{
    private Dictionary<TKey, Type> executors;

    public LogicExecutorLoader()
    {
        executors = new();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var logicExecutorType = typeof(LogicExecutorAttribute);
        var keyType = typeof(TKey);
        foreach (Assembly assembly in assemblies)
        {
            Type[] assemblyTypes = assembly.GetTypes();
            foreach (Type type in assemblyTypes)
            {
                if (!type.IsDefined(logicExecutorType)) { continue; }
                LogicExecutorAttribute attribute = (LogicExecutorAttribute)type.GetCustomAttribute(logicExecutorType, false);
                if (attribute == null || attribute.type.GetType() != keyType) { continue; }
                Debug.Log($"Add executor: {attribute.type} {type.Name}");
                executors.Add((TKey)attribute.type, type);
            }
        }
    }

    public T GetExecutor<T>(TKey key, params object[] data)
    {
        if (!executors.ContainsKey(key))
        {
            throw new ArgumentException($"There is no support for key: <{key.ToString()}>");
        }
        return (T)Activator.CreateInstance(executors[key], data);
    }
}