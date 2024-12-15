using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using UnityEngine;

public class ShopCellViewBotTap : ESCellView<ModelShopCellView>
{
    [SerializeField] private TMP_Text txtCurrPrice;
    [SerializeField] private TMP_Text txtOriginPrice;
    [SerializeField] private UIButton btnBuy;

    private int _finalPrice;
    private string _bundleId;

    public Action OnBuySuccess;

    private void OnEnable()
    {
        btnBuy.onClickEvent.AddListener(OnBuy);
    }

    private void OnDisable()
    {
        btnBuy.onClickEvent.RemoveListener(OnBuy);
    }

    public void TurnOnItem(bool isOn)
    {
        btnBuy.interactable = isOn;
    }

    public override void SetData(ModelShopCellView model)
    {
        var data = model as ModelShopCellViewContentBotTap;
        if (data==null) return;
        
        if (data.BotTapData != null)
        {
            _bundleId = data.BotTapData.id;
            txtOriginPrice.text = ((int) data.BotTapData.price).ToFormat();
            // txtSalePercentTest.text = "Test: " + data.BotTapData.sale_off_percent;
            _finalPrice = (int)data.BotTapData.GetFinalPrice();
            txtCurrPrice.text = _finalPrice.ToFormat();
        }
    }
    
    private void OnBuy()
    {
        ControllerPopup.ShowConfirmPurchase(
            string.Format(Localization.Get(TextId.Shop_AskBotPrime), Localization.Get(TextId.Common_HcName)),
            new ModelResource()
            {
                Type = TypeResource.Berry,
                Amount = _finalPrice
            }, (data,popup) =>
            {
                ProcessBuyBotPrime(popup);
            });
    }
    
    private async void ProcessBuyBotPrime(UIPopup popup)
    {
        if (!ControllerResource.IsEnough(TypeResource.Berry, _finalPrice))
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
            return;
        }
        
        this.ShowProcessing();
        try
        {
            var apiShop=FactoryApi.Get<ApiShop>();
            await apiShop.Buy(_bundleId);
            await FactoryApi.Get<ApiGame>().GetInfo();
            await FactoryApi.Get<ApiUser>().Get();
            popup.Hide();
            await apiShop.Get();
            OnBuySuccess?.Invoke();
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_BotTapPurchased));
            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }
}
