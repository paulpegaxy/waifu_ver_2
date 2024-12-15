using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class IdleEarnCardOverlayLocked : AIdleEarnCardOverlay
    {
        [SerializeField] private TMP_Text txtCondition;
        
        protected override void OnSetData()
        {
            var condition = Data.conditionData;
            if (condition == null)
                return;
            
            switch (condition.typeCondition)
            {
                case TypeConditionIdleEarnUnlock.LEVEL_UP_OTHER_CARD:
                    string conditionText = ExtensionEnum.ToIdleEarnName(condition.otherId);
                    conditionText += " ";
                    conditionText += $"Lv.{condition.valueNeed}";
                    txtCondition.text = conditionText;
                    break;
                case TypeConditionIdleEarnUnlock.INVITE_FRIEND:
                    txtCondition.text = string.Format(Localization.Get(TextId.Friend_ConditionInvite),
                        condition.valueNeed);
                    break;
            }
        }
    }
}