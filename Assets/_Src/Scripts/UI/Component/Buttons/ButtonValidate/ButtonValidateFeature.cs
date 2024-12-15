
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class ButtonValidateFeature : AButtonValidateFeature
    {
        [SerializeField] private TypeValidateFeature type;

        protected override void OnValidateFeature()
        {
            if (type == TypeValidateFeature.None)
            {
                ActiveFeature(true);
                return;
            }

            var tutorialMgr = TutorialMgr.Instance;
            bool isUnlockFeature = IsFeatureUnlocked(type, tutorialMgr);

            ActiveFeature(isUnlockFeature);
        }


        private bool IsFeatureUnlocked(TypeValidateFeature type, TutorialMgr tutorialMgr)
        {
            var tutorialManager = TutorialMgr.Instance;
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            var tutStep = gameInfo.TutorialIndex;
            switch (type)
            {
                case TypeValidateFeature.Booster:
                    if (gameInfo.PointPerTapParse > 1)
                        return true;

                    if (tutorialManager.IsUnlockTutorial(TutorialCategory.Booster))
                    {
                        TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Booster);
                        return true;
                    }

                    break;
                case TypeValidateFeature.Upgrade:
                    var apiUpgrade = FactoryApi.Get<ApiUpgrade>().Data;
                    if (apiUpgrade.IsUnlockedFirstCard)
                        return true;

                    if (tutorialManager.IsUnlockTutorial(TutorialCategory.Upgrade))
                    {
                        var firstCardPrice = apiUpgrade.GetPriceFirstCard();
                        if (gameInfo.PointParse >= firstCardPrice)
                        {
                            TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Upgrade);
                            return true;
                        }
                    }

                    break;

                case TypeValidateFeature.Message:
                case TypeValidateFeature.GameFeature:
                    return gameInfo.current_level_girl > 0;
            }

            return false;
        }

        protected override void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
        {
            if (IsMatchingCategoryAndState(category, data.State))
            {
                if (category == TutorialCategory.NextGirl)
                {
                    // UnityEngine.Debug.LogError("Vao next girl enter");
                }
                ActiveFeature(true);
            }
        }

        private bool IsMatchingCategoryAndState(TutorialCategory category, TutorialState state)
        {
            return type switch
            {
                TypeValidateFeature.Booster => category == TutorialCategory.Booster && state == TutorialState.MainBooster,
                TypeValidateFeature.Upgrade => category == TutorialCategory.Upgrade && state == TutorialState.Upgrade,
                TypeValidateFeature.Undress => category == TutorialCategory.Undress && state == TutorialState.Undress,
                TypeValidateFeature.GameFeature => category == TutorialCategory.GameFeature && state == TutorialState.FeatureRanking,
                TypeValidateFeature.Nextgirl => category == TutorialCategory.NextGirl && state == TutorialState.NextGirl,
                _ => false,
            };
        }
    }

    public enum TypeValidateFeature
    {
        None,
        Upgrade,
        Booster,
        Message,
        GameFeature,
        BotAutoTap,
        Undress,
        Nextgirl
    }
}