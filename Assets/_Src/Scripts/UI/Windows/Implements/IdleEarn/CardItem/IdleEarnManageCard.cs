// Author: ad   -
// Created: 08/08/2024  : : 01:08
// DateUpdate: 08/08/2024

using System;
using System.Linq;
using _Src.Scripts.UI.Popups;
using BreakInfinity;
using Game.Model;
using Game.Runtime;
using Sirenix.Utilities;
using Template.Defines;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public class IdleEarnManageCard : MonoBehaviour
    {
        [SerializeField] private TutorialData tutorialData;
        [SerializeField] private IdleEarnCardInfo cardInfo;
        [SerializeField] private EleIdleEarnCardOverlay[] arrCardOverlay;
        [SerializeField, ReadOnly] private string idCard;

        public static Action OnLoadedFirstCard;
        
        public void LoadData(DataIdleEarnUpgradeItem data)
        {
            idCard = data.id;
            if (data.id == "001")
            {
                //hard check communication skill
                tutorialData.SetData(TutorialObject.UpgradeConfirm);
                OnLoadedFirstCard?.Invoke();
            }
            
            cardInfo.SetData(data);
            for (var i = 0; i < arrCardOverlay.Length; i++)
            {
                arrCardOverlay[i].refData.SetData(data);
                arrCardOverlay[i].refData.gameObject.SetActive(false);
            }

            arrCardOverlay.ToList().Find(x => x.type == data.type).refData.ShowCard();
        }
    }

    public enum TypeIdleEarnCard
    {
        NONE,
        CAN_INTERACT,
        REQUIRE_CONDITION
    }
    
    [Serializable]
    public class DataIdleEarnUpgradeItem
    {
        public string id;
        public int level;
        public bool canUnlock;
        public BigDouble profitPerHour;
        public TypeResource typeCurrency;
        public BigDouble cost;
        public BigDouble profitAfter;
        public TypeIdleEarnCard type;
        public DataConditionIdleEarn conditionData;

        public DataIdleEarnUpgradeItem()
        {
        }
    }
    
    [Serializable]
    public class EleIdleEarnCardOverlay : BaseEleUIType<TypeIdleEarnCard,AIdleEarnCardOverlay>
    {
        
    }
}