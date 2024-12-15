using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;

namespace Game.Runtime
{
    public static class FactoryHelper
    {
        public static void Setup()
        {
            var factoryTypes = new List<Type>();
            var implementTypes = new List<Type>();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            foreach (Type type in assemblyTypes)
            {
                if (type.IsDefined(typeof(FactoryAttribute)))
                {
                    implementTypes.Add(type);
                }
                else if (typeof(IFactory).IsAssignableFrom(type) && !type.IsInterface && !type.IsGenericType)
                {
                    factoryTypes.Add(type);
                }
            }

            foreach (Type type in factoryTypes)
            {
                MethodInfo setup = type.GetMethod("Setup", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                setup.Invoke(null, new object[] { implementTypes });
            }

            FactoryCollection.Init();
            FactoryStorage.Init();
        }
    }
}