// Author:    -    ad
// Created: 27/07/2024  : : 3:42 PM
// DateUpdate: 27/07/2024

using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
    public class ServiceValidate : IServiceValidate
    {
        public ServiceValidate()
        {
            
        }

        public bool ValidateClick()
        {
            var stamina = ControllerResource.Get(TypeResource.ExpWaifu);
            var pointPerTap = FactoryApi.Get<ApiGame>().Data.Info.PointPerTapParse;


            return stamina.Amount > pointPerTap;
        }

        public bool ValidateUndress()
        {
            var gameInfo=FactoryApi.Get<ApiGame>().Data.Info;
            var cost = gameInfo.next_girl_level_data.CostParse;
            if (!ControllerResource.IsEnough(TypeResource.HeartPoint,cost))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughSc));
                return false;
            }
            
            // if (gameInfo.CurrentGirlId == 20009)
            // {
            //     int modLevel = (gameInfo.current_level_girl + 1) % GameConsts.MAX_LEVEL_PER_CHAR;
            //     if (modLevel == GameConsts.MAX_LEVEL_PER_CHAR - 2)
            //     {
            //         ControllerPopup.ShowInformation(Localization.Get(TextId.Confirm_MaxGirl));
            //         this.HideProcessing();
            //         return false;
            //     }
            // }

            return true;
        }

        public bool ValidateNextGirl()
        {
            var gameInfo=FactoryApi.Get<ApiGame>().Data.Info;
            if (!gameInfo.IsMaxVisual)
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotMaxGirlLevel));
                return false;
            }

            // UnityEngine.Debug.LogError("need to next: "+gameInfo.PointNeedToChangeGirlSkin);
            var cost = gameInfo.next_girl_level_data.CostParse;
            if (!ControllerResource.IsEnough(TypeResource.HeartPoint,cost))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughSc));
                return false;
            }

            var data=DBM.Config.rankingConfig.GetDataBasedCurrentGirlLevel(gameInfo.current_level_girl + 1);
            if (data == null || string.IsNullOrEmpty(data.girlName))
            {
                ControllerPopup.ShowInformation(Localization.Get(TextId.Confirm_MaxGirl));
                return false;
            }

            return true;
        }

        public bool CanUndress(ModelApiGameInfo gameInfo)
        {
            if (gameInfo.IsMaxVisual)
                return false;
            
            // if (gameInfo.PointParse >= gameInfo.CostNextGirlLevel)
            //     return true;
            
            return true;
        }

        public bool CanNextGirl()
        {
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            if (!gameInfo.IsMaxVisual)
            {
                return false;
            }

            // if (gameInfo.PointParse >= gameInfo.CostNextGirlLevel)
            // {
            //     return true;
            // }

            return true;
        }
    }
}