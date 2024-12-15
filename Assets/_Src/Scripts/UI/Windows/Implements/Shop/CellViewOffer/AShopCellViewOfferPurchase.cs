// Author: ad   -
// Created: 04/11/2024  : : 22:11
// DateUpdate: 04/11/2024

using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class AShopCellViewOfferPurchase : ShopCellViewOffer 
    {
        [SerializeField] private Image imgBackground;
        [SerializeField] private ItemTimer timerDuration;
        [SerializeField] private TMP_Text txtOriginPrice;
        [SerializeField] private TMP_Text txtCurrPrice;
        [SerializeField] private UIButton btnBuy;
        [SerializeField] private GameObject[] arrObjBuyed;
        
        protected override void SetData(ModelApiShopData data)
        {
            base.SetData(data);
            try
            {
                this.ShowProcessing();

                imgBackground.LoadSpriteAutoParseAsync(data.id);

                var timeRemain = data.end_time.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
                timerDuration.SetDuration(timeRemain);
                
                if (data.OnChainBundle)
                {
                    txtOriginPrice.text = "$" + data.price.ToDigit();
                    txtCurrPrice.text = "$" + data.GetFinalPrice().ToDigit();
                }
                else
                {
                    txtCurrPrice.text = ((int) data.GetFinalPrice()).ToFormat();
                    txtOriginPrice.text = ((int) data.price).ToFormat();
                }

                btnBuy.gameObject.SetActive(!data.IsReachLimit);
                TurnOnBuyed(data.IsReachLimit);
                
                OnSetData();

                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
        
        private void OnEnable()
        {
            btnBuy.onClickEvent.AddListener(OnBuy);
        }

        private void OnDisable()
        {
            btnBuy.onClickEvent.RemoveListener(OnBuy);
            TurnOnBuyed(false);
        }
        
        private void TurnOnBuyed(bool isOn)
        {
            for (int i = 0; i < arrObjBuyed.Length; i++)
            {
                arrObjBuyed[i].SetActive(isOn);
            }
        }

        protected abstract void OnSetData();
        
        private void OnBuy()
        {
            if (Data.OnChainBundle)
                ProcessPurchaseOnChain();
            else
                ProcessPurchaseInGameBundle();
        }
        
        protected virtual async UniTask OnCompetePurchase()
        {
            ControllerShopOffer.ReCheckAllOffer();
        }

        
        
        private void ProcessPurchaseInGameBundle()
        {
            if (!ControllerResource.IsEnough(TypeResource.Berry, Data.GetFinalPrice()))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
                return;
            }
            
            async void ProcessPurchase(UIPopup popup)
            {
                this.ShowProcessing();
                try
                {
                    var apiShop=FactoryApi.Get<ApiShop>();
                    await apiShop.Buy(Data.id);
                    await apiShop.Get();
                    await FactoryApi.Get<ApiGame>().GetInfo();
                    
                    await FactoryApi.Get<ApiUpgrade>().Get();
                    // ControllerUI.Instance.Spawn(_itemReward.IdResource, transform.position, 20);
                    ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_SuccessPurchased));
                    TurnOnBuyed(true);
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
            ControllerPopup.ShowConfirmPurchase(des,new ModelResource()
            {
                Type = TypeResource.Berry,
                Amount = Data.GetFinalPrice()
            }, (data, popupConfirm) =>
            {
                ProcessPurchase(popupConfirm);
            });
        }

        private async void ProcessPurchaseOnChain()
        {
            this.ShowProcessing();
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();

                await apiShop.BuyWithTon(Data.id);
                
                await FactoryApi.Get<ApiGame>().GetInfo();
                await FactoryApi.Get<ApiEvent>().Get();
                
                await apiShop.Get();
                await OnCompetePurchase();
                TurnOnBuyed(true);
                
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
}