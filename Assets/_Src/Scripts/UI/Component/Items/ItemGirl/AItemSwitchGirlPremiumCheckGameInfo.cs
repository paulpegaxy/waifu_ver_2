// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public abstract class AItemSwitchGirlPremiumCheckGameInfo : AItemSwitchGirlPremium
    {
        protected override void OnEnabled()
        {
            base.OnEnabled();
            ModelApiGameInfo.OnChanged += OnGameInfoChanged;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
        }

        protected virtual void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
            if (!userInfo.isChoosePremiumWaifu)
            {
                if (InitGirlId != gameInfo.CurrentGirlId)
                {
                    InitGirlId = gameInfo.CurrentGirlId;
                    SwitchGirl(false);
                }
            }
        }

    }
}