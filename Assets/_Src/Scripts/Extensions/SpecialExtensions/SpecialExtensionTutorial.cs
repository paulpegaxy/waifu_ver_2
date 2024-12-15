using System;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.Extensions
{
    public static class SpecialExtensionTutorial
    {
        public static bool IsPassTutorial(TutorialCategory category)
        {
            var apiGame = FactoryApi.Get<ApiGame>();
            // if (apiGame.Data.Info == null)
            //     return false;
            
            if (apiGame.Data.Info.current_level_girl >= GameConsts.MAX_LEVEL_PER_CHAR)
            {
                return true;
            }

            var dataTuts = apiGame.Data.Info.TutorialIndex;
            // if (dataTuts >= (int)TutorialCategory.NextGirl || dataTuts >= (int)category)
                if (dataTuts >= (int)TutorialCategory.GameFeature || dataTuts >= (int)category)
                return true;

            return false;
        }

        public static bool IsInUndressTutorial()
        {
            if (IsPassTutorial(TutorialCategory.Undress))
                return false;

            var gameInfo= FactoryApi.Get<ApiGame>().Data.Info;
            if (gameInfo.TutorialIndex == (int)TutorialCategory.Upgrade || IsPassTutorial(TutorialCategory.Upgrade))
                return true;
            
            return false;
        }

        public static bool IsInPrevBoosterTutorial()
        {
            if (IsPassTutorial(TutorialCategory.Booster))
                return false;
            
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            var currStep = TutorialMgr.Instance.GetTutorialState(TutorialCategory.Main);
            if (gameInfo.TutorialIndex <= (int)TutorialCategory.Main && currStep >= TutorialState.MainPointCurrency)
                return true;

            return false;
        }

        public static int GetUndressTapCount()
        {
            try
            {
                var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
                int cost = gameInfo.next_girl_level_data.cost;
                int point = int.Parse(gameInfo.point);
              
                if (point >= cost)
                    return 0;
                
                int final = Mathf.FloorToInt((cost - point) / int.Parse(gameInfo.point_per_tap));
                return Mathf.Clamp(final, 1, GameConsts.MAX_LOG_TAP_FOR_GAME_FEATURE_TUT);
            }
            catch (Exception e)
            {
                GameUtils.Log("white", e.Message);
                return 0;
            }
        }
        
        public static bool IsInTutorial(TutorialCategory category, TutorialState step)
        {
            if (IsPassTutorial(category))
                return false;
        
            if (TutorialMgr.Instance.IsState(category, step))
                return true;

            return false;
        }

    }
}