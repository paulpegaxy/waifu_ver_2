using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using DG.Tweening.Plugins.Core.PathCore;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using UnityEngine;

public class ItemProfitPerHour : GameplayListener
{
    // [SerializeField] private ItemAvatar itemAvatar;
    [SerializeField] private TMP_Text txtProfit;
    [SerializeField] private TMP_Text txtPercent;
    [SerializeField] private UIButton btnInfo;
    
    protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
    {
        LoadData(gameInfo);
    }

    protected override void OnInitialize()
    {
        LoadData(FactoryApi.Get<ApiGame>().Data.Info);
    }

    private void LoadData(ModelApiGameInfo gameInfo)
    {
        // var profitPerHour = Mathf.FloorToInt(gameInfo.point_per_second * 3600);
        
        // txtProfit.text = "+" + gameInfo.PointPerHourParse.ToLetter();
        // var profitPerHour =BigDouble.Parse(gameInfo.ProfitPerHourParse.ToString()

        txtProfit.text = "+" + gameInfo.ProfitPerHourParse.ToFormat();
        if (gameInfo.current_girl_bonus > 0)
        {
            txtPercent.transform.parent.gameObject.SetActive(true);
            txtPercent.text = $"{gameInfo.current_girl_bonus}%";
            // itemAvatar.gameObject.SetActive(true);
        }
        else
        {
            txtPercent.transform.parent.gameObject.SetActive(false);
            // itemAvatar.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        btnInfo.onClickEvent.AddListener(OnClickInfo);
    }

    private void OnDestroy()
    {
        btnInfo.onClickEvent.RemoveListener(OnClickInfo);
    }

    private void OnClickInfo()
    {
        this.ShowPopup(UIId.UIPopupName.PopupGuideProfitPerHour);
    }
}
