using System;
using BreakInfinity;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;

public class PopupBotTap : MonoBehaviour
{
    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private TMP_Text txtRemainTrial;
    [SerializeField] private TMP_Text txtOriginPrice;
    [SerializeField] private TMP_Text txtCurrPrice;

    [SerializeField] private UIButton btnTrial;
    [SerializeField] private UIButton btnPremium;

    private ModelApiShopData _dataBotPrime;
    private bool _canUseBotTrial;

    private void OnEnable()
    {
        btnTrial.onClickEvent.AddListener(OnTrial);
        btnPremium.onClickEvent.AddListener(OnPremium);
        LoadData();
    }

    private void LoadData()
    {
        var apiShop = FactoryApi.Get<ApiShop>().Data;
        var botPrime = apiShop.GetItemByItemType(TypeShopItem.TapBotPrime);
        if (botPrime != null)
        {
            _dataBotPrime = botPrime;
            txtOriginPrice.text = ((int)botPrime.price).ToFormat();
            txtCurrPrice.text = ((int)botPrime.GetFinalPrice()).ToFormat();
        }

        var botTrial = FactoryApi.Get<ApiUser>().Data.Game.auto_bot;
        txtTime.text = botTrial.trial_auto_bot_time.ToTime();
        _canUseBotTrial = botTrial.trial_auto_bot_used_today < botTrial.trial_auto_bot_limit;
        txtRemainTrial.text = $"{botTrial.trial_auto_bot_used_today}/{botTrial.trial_auto_bot_limit}";
        txtRemainTrial.color = _canUseBotTrial ? Color.green : Color.red;
    }

    private void OnDisable()
    {
        btnTrial.onClickEvent.RemoveListener(OnTrial);
        btnPremium.onClickEvent.RemoveListener(OnPremium);
    }

    private void OnTrial()
    {
        if (!_canUseBotTrial)
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FullBotTrial));
            return;
        }

        ControllerPopup.ShowConfirm(Localization.Get(TextId.Shop_AskBotTrial), onOk: (popup) =>
        {
            // #if UNITY_EDITOR
            SpecialExtensionGame.PlayBotTrial(() =>
            {
                GetComponent<UIPopup>().Hide();
            }, popup);
            // #else
            //             ProcessWatchAdsToTrialBot(popup);
            // #endif

        });
    }

    private void OnPremium()
    {
        if (_dataBotPrime == null)
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_UnavailableBuy));
            return;
        }

        ControllerPopup.ShowConfirmPurchase(
            string.Format(Localization.Get(TextId.Shop_AskBotPrime), Localization.Get(TextId.Common_HcName)),
            new ModelResource()
            {
                Type = TypeResource.Berry,
                Amount = BigDouble.Parse(((int)_dataBotPrime.GetFinalPrice()).ToString())
            }, (data, popup) => { ProcessBuyBotPrime(popup); });
    }

    private void ActiveBot()
    {
        var storageSetting = FactoryStorage.Get<StorageSettings>();
        storageSetting.Get().isUseBotTap = true;
        storageSetting.Save();
    }

    private async void ProcessWatchAdsToTrialBot(UIPopup popup)
    {
        var test = 2744;
        try
        {
            var result = await Adsgram.Show(test);
            if (result.done && !result.error)
            {
                ControllerPopup.ShowToast("Watch ads success");
                SpecialExtensionGame.PlayBotTrial(popupConfirm: popup);
            }
            else
            {
                if (!string.IsNullOrEmpty(result.description))
                {
                    popup.Hide();
                    ControllerPopup.ShowToast(result.description);
                }
                else
                {
                    popup.Hide();
                    ControllerPopup.ShowToast("Failed to watch ads");
                }
            }
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }


    private async void ProcessBuyBotPrime(UIPopup popup)
    {
        if (!ControllerResource.IsEnough(TypeResource.Berry, _dataBotPrime.GetFinalPrice()))
        {
            ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
            return;
        }

        this.ShowProcessing();
        try
        {
            ActiveBot();
            await FactoryApi.Get<ApiShop>().Buy(TypeShopItem.TapBotPrime);
            await FactoryApi.Get<ApiGame>().GetInfo();
            await FactoryApi.Get<ApiUser>().Get();
            popup.Hide();
            GetComponent<UIPopup>().Hide();
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_BotTapPurchased));

            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }
}
