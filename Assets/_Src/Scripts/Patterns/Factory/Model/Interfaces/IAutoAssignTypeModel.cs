using System;

namespace Game.Model.Utils
{
    public interface IAutoAssignTypeModel<TEnum> where TEnum : Enum
    {
        void AutoAssignType(TEnum type);
    }
}