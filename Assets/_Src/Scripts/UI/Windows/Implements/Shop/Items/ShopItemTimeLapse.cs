using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTimeLapse : MonoBehaviour
{
    [SerializeField] private Image imgItem;
    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private TMP_Text txtDes;
    [SerializeField] private TMP_Text txtPrice;
    [SerializeField] private TMP_Text txtReceiveValue;
    [SerializeField] private UIButton btnClick;
    [SerializeField] private Color colorHighLight;

    private ModelApiShopData _data;
    private ModelApiItemData _itemReward;

    public Action OnBuySuccess;
    
    private void Awake()
    {
        btnClick.onClickEvent.AddListener(OnClick);
    }
    
    private void OnDestroy()
    {
        btnClick.onClickEvent.RemoveListener(OnClick);
    }

    public void TurnOnItem(bool isOn)
    {
        btnClick.interactable = isOn;
    }

    public void SetData(ModelApiShopData data)
    {
        _data = data;

        txtPrice.text = ((int) _data.GetFinalPrice()).ToFormat();
        
        ProcessTimelapseContent(data);
    }

    private void ProcessTimelapseContent(ModelApiShopData data)
    {
        string strTime = "";
        
        if (data.GetPackType() == TypeShopPack.TimeLapse)
        {
            var key = data.client_data[^1].Split('_');
            ExtensionImage.LoadShopTimelapse(imgItem, key.Length > 0 ? key[0] : "").Forget();
            if (key.Length > 0)
            {
                strTime = key[^1];
            }
        }

        if (string.IsNullOrEmpty(strTime))
        {
            strTime = data.id.Split('_')[^1];
        }

        txtTime.text = strTime.ToUpper();

        string value = "NaN";
        if (_data.items.Count > 0)
        {
            _itemReward = _data.items[0];
            value = _itemReward.ValueParse.ToLetter();
        }

        string highlightValueStr = strTime.Replace("H", "").SetHighlightString(colorHighLight);
        txtDes.text = string.Format(Localization.Get(TextId.Shop_DesTimelapse), highlightValueStr.Replace("h", ""));
        txtReceiveValue.text = value;
    }

    private void OnClick()
    {
        if (_data == null)
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_UnavailableBuy));
            return;
        }

        int price = (int) _data.GetFinalPrice();
        string description = string.Format(Localization.Get(TextId.Shop_AskBuyTimelapse), price.ToFormat(), Localization.Get(TextId.Common_HcName), txtTime.text);
        
        var popup = this.ShowPopup<PopupConfrmPurchasePrice>(UIId.UIPopupName.PopupConfirmPurchasePrice);
        popup.SetData(description, new ModelResource()
        {
            Type = TypeResource.Berry,
            Amount = _data.GetFinalPrice()
        }, (data, popupConfirm) =>
        {
            ProcessBuyTimeLapse(popupConfirm);
        });
        popup.SetValueReceived(new ModelResource()
        {
            Type = TypeResource.HeartPoint,
            Amount = _itemReward.ValueParse
        });
    }

    private async void ProcessBuyTimeLapse(UIPopup popup)
    {
        if (!ControllerResource.IsEnough(TypeResource.Berry, _data.GetFinalPrice()))
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
            return;
        }
        
        this.ShowProcessing();
        try
        {
            var apiShop=FactoryApi.Get<ApiShop>();
            await apiShop.Buy(_data.id);
            await apiShop.Get();
            await FactoryApi.Get<ApiGame>().GetInfo();
            ControllerUI.Instance.Spawn(_itemReward.IdResource, transform.position, 20);
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_SuccessPurchased));
            popup.Hide();
            OnBuySuccess?.Invoke();
            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }
}
