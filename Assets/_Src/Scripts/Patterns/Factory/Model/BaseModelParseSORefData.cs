using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Model
{
    [Serializable]
    public abstract class BaseModelParseSORefData<TData> where TData : ScriptableObject
    {
        [field: SerializeField] public string fileName;

        [ShowInInspector, JsonIgnore] private TData data;

        public TData Data => data;

        public void SetData(TData data)
        {
            this.data = data;
        }
    }
}