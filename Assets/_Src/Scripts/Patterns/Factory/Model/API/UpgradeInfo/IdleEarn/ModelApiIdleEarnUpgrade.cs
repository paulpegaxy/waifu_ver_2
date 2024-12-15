// Author: ad   -
// Created: 07/08/2024  : : 23:08
// DateUpdate: 07/08/2024

using System;
using BreakInfinity;
using UnityEngine;

namespace Game.Model
{
    
    [Serializable]
    public class ModelApiIdleEarnUpgrade
    {
        public ModelApiIdleEarnUpgradeCurrent current;
        public ModelApiIdleEarnUpgradeNext next;
        
        public BigDouble CurrentPointPerHour
        {
            get
            {
                string value = string.IsNullOrEmpty(current.point_per_hour) ? "0" : current.point_per_hour;
                return BigDouble.Parse(value);
            }
        }

        public BigDouble NextPointPerHour => next.AddParse + CurrentPointPerHour;
    }

    [Serializable]
    public class ModelApiIdleEarnUpgradeCurrent
    {
        public string id;
        public int level;
        public string point_per_hour;
        public bool unlocked;
        public string unlock_condition;

        public DataConditionIdleEarn ConditionData => string.IsNullOrEmpty(unlock_condition) ? 
            null : 
            new DataConditionIdleEarn(unlock_condition);
    }
    
    [Serializable]
    public class ModelApiIdleEarnUpgradeNext : ModelApiUpgradeCostParse
    {
        public string add;

        public BigDouble AddParse => BigDouble.Parse(add);
    }

    [Serializable]
    public class DataConditionIdleEarn
    {
        public TypeConditionIdleEarnUnlock typeCondition;
        public string otherId;
        public int valueNeed;

        public DataConditionIdleEarn(string conditionString)
        {
            var arrParse = conditionString.Split(';');
            TypeConditionIdleEarnUnlock type = arrParse[0].Equals("invite")
                ? TypeConditionIdleEarnUnlock.INVITE_FRIEND
                : TypeConditionIdleEarnUnlock.LEVEL_UP_OTHER_CARD;
            
            typeCondition = type;
            valueNeed = int.Parse(arrParse[^1]);
            if (typeCondition == TypeConditionIdleEarnUnlock.LEVEL_UP_OTHER_CARD)
                otherId = arrParse[1];
        }
    }

    public enum TypeConditionIdleEarnUnlock
    {
        NONE,
        LEVEL_UP_OTHER_CARD,
        INVITE_FRIEND
    }
}