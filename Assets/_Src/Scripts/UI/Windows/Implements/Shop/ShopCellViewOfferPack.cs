using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCellViewOfferPack : ESCellView<ModelShopCellView>
{
    [SerializeField] private TMP_Text txtOriginPrice;
    [SerializeField] private TMP_Text txtCurrPrice;
    [SerializeField] private TMP_Text txtLimit;
    [SerializeField] private TMP_Text txtPercent;
    [SerializeField] private GameObject objSoldOut;
    [SerializeField] private Image imgHolder;
    [SerializeField] private UIButton btnBuy;
    [SerializeField] private TMP_Text txtHc;
    [SerializeField] private TMP_Text txtTimelapse;
    [SerializeField] private Image imgTimelapse;

    [SerializeField] private GameObject[] arrObjItem;

    private ModelApiShopData _data;

    private void OnEnable()
    {
        btnBuy.onClickEvent.AddListener(OnClick);
    }

    private void OnDisable()
    {
        btnBuy.onClickEvent.RemoveListener(OnClick);
    }

    public void SetData(ModelApiShopData data)
    {
        _data = data;
        if (_data.OnChainBundle)
        {
            txtOriginPrice.text = "$" + _data.price.ToDigit();
            txtCurrPrice.text = "$" + _data.GetFinalPrice().ToDigit();
        }
        else
        {
            txtCurrPrice.text = ((int)_data.GetFinalPrice()).ToFormat();
            txtOriginPrice.text = ((int)_data.price).ToFormat();
        }

        if (data.GetPackType() == TypeShopPack.TimeLapse)
        {
            if (data.items.Count > 1)
            {
                arrObjItem[1].SetActive(true);
                ProcessTimelapseContent(data);
            }
        }

        txtHc.text = _data.items[0].ValueParse.ToLetter();
        txtLimit.text = _data.GetLimitText();
        txtPercent.text = _data.GetPercentDiscount() + "%";

        imgHolder.DOFade(0, 0);
        imgHolder.LoadSpriteAutoParseAsync(_data.client_data[1]);
        btnBuy.interactable = !data.IsReachLimit;
        objSoldOut.SetActive(data.IsReachLimit);

    }

    private void ProcessTimelapseContent(ModelApiShopData data)
    {
        string strTime = "";

        if (data.GetPackType() == TypeShopPack.TimeLapse)
        {
            var key = data.client_data[^1].Split('_');
            ExtensionImage.LoadShopTimelapse(imgTimelapse, key.Length > 0 ? key[0] : "").Forget();
            if (key.Length > 0)
            {
                strTime = key[^1];
            }
        }

        if (string.IsNullOrEmpty(strTime))
        {
            strTime = data.id.Split('_')[^1];
        }

        txtTimelapse.text = strTime.ToUpper();
    }

    public override void SetData(ModelShopCellView data)
    {
        if (data is ModelShopCellViewContentOfferPack modelData)
        {
            SetData(modelData.OfferPackData);
        }
    }

    protected void OnClick()
    {
        if (!_data.OnChainBundle)
        {
            ProcessPurchaseInGameBundle();
        }
        else
        {
            string des = string.Format(Localization.Get(TextId.Shop_AskBuyItem), $"${_data.GetFinalPrice().ToDigit()}");
            ControllerPopup.ShowConfirm(des, Localization.Get(TextId.Common_Confirm), onOk: ProcessPurchaseOnChain);
        }
    }

    protected virtual async UniTask OnCompetePurchase()
    {

    }

    private void ProcessPurchaseInGameBundle()
    {
        if (!ControllerResource.IsEnough(TypeResource.Berry, _data.GetFinalPrice()))
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
            return;
        }

        async void ProcessPurchase(UIPopup popup)
        {
            this.ShowProcessing();
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();
                await apiShop.Buy(_data.id);
                await apiShop.Get();
                await FactoryApi.Get<ApiGame>().GetInfo();

                await FactoryApi.Get<ApiUpgrade>().Get();
                // ControllerUI.Instance.Spawn(_itemReward.IdResource, transform.position, 20);
                ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_SuccessPurchased));

                objSoldOut.SetActive(true);
                await OnCompetePurchase();
                popup.Hide();

                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        string des = string.Format(Localization.Get(TextId.Event_ConfirmBuyBundle),
            Localization.Get(TextId.Common_HcName));
        ControllerPopup.ShowConfirmPurchase(des, new ModelResource()
        {
            Type = TypeResource.Berry,
            Amount = _data.GetFinalPrice()
        }, (data, popupConfirm) => { ProcessPurchase(popupConfirm); });
    }

    private async void ProcessPurchaseOnChain(UIPopup popup)
    {
        this.ShowProcessing();
        try
        {
            var apiShop = FactoryApi.Get<ApiShop>();

            await apiShop.BuyWithTon(_data.id);

            await FactoryApi.Get<ApiGame>().GetInfo();
            await FactoryApi.Get<ApiEvent>().Get();
            await apiShop.Get();
            await OnCompetePurchase();
            popup.Hide();
            objSoldOut.SetActive(true);

            ControllerPopup.ShowToast(Localization.Get(TextId.Shop_SuccessPurchased));
            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
            ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedPurchased));
        }
    }
}