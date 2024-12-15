using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ExtensionClassProperties
{
    public static Dictionary<string, object> GetPropertiesDataInClass<T>(T classData, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
    {
        return classData.GetType().GetFields(flags).ToList()
            .ToDictionary(key => key.Name, field => field.GetValue(classData));
    }
}
