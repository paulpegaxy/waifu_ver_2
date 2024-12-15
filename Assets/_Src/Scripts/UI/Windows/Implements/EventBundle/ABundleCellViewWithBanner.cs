// Author: ad   -
// Created: 29/10/2024  : : 23:10
// DateUpdate: 29/10/2024

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
    public abstract class ABundleCellViewWithBanner : ESCellView<AModelEventBundleCellView>
    {
        [SerializeField] protected Image imgBanner;
        [SerializeField] protected UIButton btnClick;
        [SerializeField] protected GameObject objSoldOut;
        [SerializeField] protected TMP_Text txtOriginPrice;
        [SerializeField] protected TMP_Text txtCurrentPrice;
        
        private ModelApiShopData _data;

        private void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
        }

        public override void SetData(AModelEventBundleCellView data)
        {
            _data = data.DataBundle;
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
                ControllerPopup.ShowConfirm(des,Localization.Get(TextId.Common_Confirm) ,onOk: ProcessPurchaseOnChain);
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
                    var apiShop=FactoryApi.Get<ApiShop>();
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
            ControllerPopup.ShowConfirmPurchase(des,new ModelResource()
            {
                Type = TypeResource.Berry,
                Amount = _data.GetFinalPrice()
            }, (data, popupConfirm) =>
            {
                ProcessPurchase(popupConfirm);
            });
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
}