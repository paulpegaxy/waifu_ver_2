// Author:    -    ad
// Created: 27/07/2024  : : 3:42 PM
// DateUpdate: 27/07/2024

using Game.Model;

namespace Game.Runtime
{
    public interface IServiceValidate : IService
    {
        bool ValidateClick();
        bool ValidateUndress();
        bool ValidateNextGirl();
        bool CanNextGirl();
        bool CanUndress(ModelApiGameInfo gameInfo);
    }
}