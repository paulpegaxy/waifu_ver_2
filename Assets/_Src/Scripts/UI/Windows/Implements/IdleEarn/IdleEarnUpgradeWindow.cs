using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;

public class IdleEarnUpgradeWindow : UIWindow
{
    [SerializeField] private IdleEarnUpgradeScroller scroller;

    protected override async void OnEnabled()
    {
        IdleEarnManageCard.OnLoadedFirstCard += OnCompleteLoadFirstCard;
        ModelApiUpgradeInfo.OnChanged += OnModelApiUpgradeInfoChanged;
        await FactoryApi.Get<ApiUpgrade>().Get();
    }

    protected override void OnDisabled()
    {
        ModelApiUpgradeInfo.OnChanged -= OnModelApiUpgradeInfoChanged;
        IdleEarnManageCard.OnLoadedFirstCard-= OnCompleteLoadFirstCard;
    }
    private void OnModelApiUpgradeInfoChanged(ModelApiUpgradeInfo obj)
    {
        ReloadShow();
    }

    private void ReloadShow()
    {
        var apiUpgrade = FactoryApi.Get<ApiUpgrade>().Data.upgrade;
        var listData = apiUpgrade.Select(item => new DataIdleEarnUpgradeItem
        {
            id = item.current.id,
            level = item.current.level,
            canUnlock = item.current.unlocked,
            cost = item.next.CostParse,
            typeCurrency = TypeResource.HeartPoint,
            profitPerHour = item.CurrentPointPerHour,
            profitAfter = item.NextPointPerHour,
            type = item.current.unlocked? TypeIdleEarnCard.CAN_INTERACT : TypeIdleEarnCard.REQUIRE_CONDITION,
            conditionData = item.current.ConditionData
        }).ToList();
        
        scroller.SetData(listData);
        
    }
    
    private void OnCompleteLoadFirstCard()
    {
        OnDataLoaded?.Invoke(UIId.UIViewCategory.Window, UIId.UIViewName.IdleEarn);
    }
}
