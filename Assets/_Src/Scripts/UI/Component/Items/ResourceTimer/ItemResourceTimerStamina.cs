using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Debug;
using Game.Model;
using Game.Runtime;
using Template.Defines;

public class ItemResourceTimerStamina : AItemResourceTimer
{

    protected override void UpdateProgress(object obj)
    {
        if (FactoryApi.Get<ApiGame>().Data.Info == null)
        {
            return;
        }
        
        var apiGame = FactoryApi.Get<ApiGame>().Data.Info;
        ControllerResource.Add(TypeResource.ExpWaifu, apiGame.stamina_per_second);
        if (ControllerResource.Get(TypeResource.ExpWaifu).Amount >= apiGame.stamina_max)
        {
            apiGame.stamina = apiGame.stamina_max.ToString();
            ControllerResource.Set(TypeResource.ExpWaifu, apiGame.stamina_max);
        }
    }
}