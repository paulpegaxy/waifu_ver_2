// Author: ad   -
// Created: 17/10/2024  : : 06:10
// DateUpdate: 17/10/2024

using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ItemSwitchGirlPremiumLevelText : AItemSwitchGirlPremium
    {
        [SerializeField] private TMP_Text txtLevel;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            ModelApiGameInfo.OnChanged += OnGameInfoChanged;    
            ModelApiUpgradeInfo.OnChanged += OnUpgradeInfoChanged;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            ModelApiUpgradeInfo.OnChanged -= OnUpgradeInfoChanged;
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
        }
        
        private void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            if (!IsPremium)
            {
                SwitchGirl(false);
            }
        }
        
        private void OnUpgradeInfoChanged(ModelApiUpgradeInfo upgradeInfo)
        {
            if (IsPremium)
            {
                SwitchGirl(true);
            }
        }

        protected override void SwitchGirl(bool isPremium)
        {
            if (isPremium)
            {
                var apiUpgrade = FactoryApi.Get<ApiUpgrade>().Data.GetPremiumChar(InitGirlId);
                ReloadLevel(apiUpgrade.level - 1);
            }
            else
            {
                var gameInfo= FactoryApi.Get<ApiGame>().Data.Info;
                ReloadLevel(gameInfo.current_level_girl);
            }
        }

        private void ReloadLevel(int level)
        {
            var currentCharLevel = level;
            var levelDisplay = currentCharLevel % GameConsts.MAX_LEVEL_PER_CHAR;
            txtLevel.text = $"<color=white>LV.{(levelDisplay + 1):00}</color>/{GameConsts.MAX_LEVEL_PER_CHAR}";
        }
    }
}