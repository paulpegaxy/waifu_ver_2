using System;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.UI
{
    public class ItemBubbleGirlNotify : AItemCheckStartGame
    {
        [SerializeField] private GameObject objHolder;
        [SerializeField] private TMP_Text txtMessage;


        protected override void Awake()
        {
            base.Awake();
            objHolder.SetActive(false);
        }

        protected override void OnEnabled()
        {
            TutorialStep.OnExit += OnTutorialStepExit;
        }

        protected override void OnDisabled()
        {
            TutorialStep.OnExit -= OnTutorialStepExit;
        }

        protected override void OnInit()
        {
            if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.NextGirl))
            {
                objHolder.SetActive(false);
            }

            if (SpecialExtensionTutorial.IsInUndressTutorial())
            {
                var count = SpecialExtensionTutorial.GetUndressTapCount();
                if (count > 0)
                    SetText(count);
            }
            else
            {
                if (SpecialExtensionTutorial.IsInPrevBoosterTutorial())
                {
                    SetText(GameConsts.MAX_LOG_TAP_FOR_BOOSTER_TUT);
                }
            }
        }

        private void SetText(int number)
        {
            txtMessage.text = $"Please tap\n{number} times";
            objHolder.SetActive(true);
        }

        private void OnTutorialStepExit(TutorialCategory category, ModelTutorialStep step)
        {
            objHolder.SetActive(false);
            switch (category)
            {
                case TutorialCategory.Main:
                    if (step.State == TutorialState.MainFirstTimeLogin)
                    {
                        SetText(GameConsts.MAX_LOG_TAP_FOR_FIRST_TIME);
                    }
                    else if (step.State == TutorialState.MainPointCurrency)
                    {
                        SetText(GameConsts.MAX_LOG_TAP_FOR_BOOSTER_TUT);
                    }

                    break;
                case TutorialCategory.Upgrade:
                    if (step.State == TutorialState.UpgradeGuideProfit)
                    {
                        SetText(GameConsts.MAX_LOG_TAP_FOR_GAME_FEATURE_TUT);
                    }
                    break;
                case TutorialCategory.Undress:
                    if (step.State == TutorialState.Undress)
                    {
                        SetText(GameConsts.MAX_LOG_TAP_FOR_GAME_FEATURE_TUT);
                    }
                    break;
            }
        }
    }
}