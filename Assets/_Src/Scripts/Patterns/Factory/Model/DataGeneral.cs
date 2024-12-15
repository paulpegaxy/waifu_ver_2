using System;
using Game.Data;
using Template.Defines;
using UnityEngine;

namespace Game.Model.Data
{
    public class DataGeneral
    {

    }

    [Serializable]
    public class DataItemValuePair : IResourceItem
    {
        [field: SerializeField] public TypeResource Type { get; set; }
        [field: SerializeField] public float Value { get; set; }
    }

    [Serializable]
    public abstract class DataVectorMinMax<T>
    {
        public T min;
        public T max;
    }
}