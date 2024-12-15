using System;

namespace _Src.Scripts.UI.Popups
{
    [Serializable]
    public abstract class BaseEleUIType<TEnum,TElement> where  TEnum : Enum
    {
        public TEnum type;
        public TElement refData;
    }
}