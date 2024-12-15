using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Runtime;
using Template.Defines;

public class ItemResourceTimerGold : AItemResourceTimer
{

    protected override void UpdateProgress(object obj)
    {
        // if (DataGameInfo == null)
        // {
        //     return;
        // }
        // if (DataGameInfo.PointPerSecondParse <= 0) return;
        //
        // ControllerResource.Add(TypeResource.HeartPoint, DataGameInfo.PointPerSecondParse);
    }
}