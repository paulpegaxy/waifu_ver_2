

using UnityEngine;

namespace Game.Debug
{
    public static class UserSessionData
    {
        public const int baseTapPoint = 1;
        public const int baseStaminaMax = 2000;
        public const int baseStaminaRecovery = 3;

        public const int baseStaminaPerUpgrade = 500;
        public const int baseTapPointPerUpgrade = 1;

        public const int STAMINA_PER_SECOND = 3;

        public static int CostUpgradeStaminaLimit(int level)
        {
            return 1000 * (int) Mathf.Pow(level, level - 1);
        }
        
        public static int CostUpgradeMulti(int level)
        {
            return 1000 * (int) Mathf.Pow(level, level - 1);
        }

        public static int GetStaminaMaxAtLevel(int level)
        {
            return baseStaminaMax + (level * baseStaminaPerUpgrade);
        }

        public static int GetTapPointAtLevel(int level)
        {
            return baseTapPoint + (level * baseTapPointPerUpgrade);
        }
    }
}