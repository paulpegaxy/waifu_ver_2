using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using Template.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupConfirmPurchaseSkipTime : MonoBehaviour
{
    [SerializeField] private ItemTimer itemTimer;
    [SerializeField] private TMP_Text textPrice;
    [SerializeField] private TMP_Text txtPriceUpgrade;
    [SerializeField] private UIButton btnBuy;

    private ModelApiUpgradePremiumChar _charData;
    
    private void OnEnable()
    {
        btnBuy.onClickEvent.AddListener(OnBuy);
    }
    
    private void OnDisable()
    {
        btnBuy.onClickEvent.RemoveListener(OnBuy);
    }

    public void Show(ModelApiUpgradePremiumChar data)
    {
        _charData = data;
        textPrice.text = data.next.berry_to_skip.ToFormat();
        txtPriceUpgrade.text = data.next.CostParse.ToLetter();
        itemTimer.SetDuration(data.GetCooldownUndress(), Reload);
    }
    
    private async void OnBuy()
    {
        try
        {
            if (!ControllerResource.IsEnough(TypeResource.Berry, _charData.next.berry_to_skip))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Gallery_NotiCantSkipTime));
                return;
            }

            if (!ControllerResource.IsEnough(TypeResource.HeartPoint, _charData.next.CostParse))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughSc));
                return;
            }
            
            this.ShowProcessing();
            await FactoryApi.Get<ApiUpgrade>().PostSkipCooldown(_charData.id,true);
            FactoryApi.Get<ApiGame>().GetInfo().Forget();
            GetComponent<UIPopup>().Hide();
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_SuccessPurchased));
            
            this.PostEvent(TypeGameEvent.UndressGirl);
            
            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }

    private async void Reload()
    {
        await FactoryApi.Get<ApiUpgrade>().Get();
        GetComponent<UIPopup>().Hide();
    }
}
