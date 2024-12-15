using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ShopCellViewSubscription : ESCellView<ModelShopCellView>
    {
        [SerializeField] private UIButton btnBuy;
        [SerializeField] private Image imgIconSub;
        [SerializeField] private Image imgIconSubTag;
        [SerializeField] private TMP_Text txtPrice;
        [SerializeField] private TMP_Text txtSwipeInstantly;
        [SerializeField] private TMP_Text txtSwipeRefresh;
        
        public static Action<bool> OnSubscribe;

        private ModelApiSubscriptionConfig _data;
        
        public override void SetData(ModelShopCellView model)
        {
            if (model is ModelShopCellViewContentSubscription data)
            {
                _data = data.Config;
                imgIconSub.sprite = ControllerSprite.Instance.GetSubPack(data.Index);
                imgIconSub.SetNativeSize();
                imgIconSubTag.sprite = ControllerSprite.Instance.GetSubTag(data.Index);
                imgIconSubTag.SetNativeSize();
                SetPrice(_data.GetStarPrice());
                try
                {
                    txtSwipeInstantly.text = $"+{_data.GetFirstItemValue()} swipes instantly";
                    txtSwipeRefresh.text = $"+{_data.GetSecondItemValue()} swipes every day";
                }
                catch (Exception e)
                {
                    e.ShowError();
                }
            }
        }

        private void SetPrice(float price)
        {
            txtPrice.text = $"<color=#6263EB>{price}</color>";
            txtPrice.text+= "<color=#292A30>/month</color>";
        }
        
        private void OnEnable()
        {
            btnBuy.onClickEvent.AddListener(OnBuy);
        }

        private void OnDisable()
        {
            btnBuy.onClickEvent.RemoveListener(OnBuy);
        }
        
        private async void OnBuy()
        {
            this.ShowProcessing();
            var apiUser = FactoryApi.Get<ApiUser>();
            try
            {
                var data=await apiUser.PostBuySubscription(_data.id);
                if (data != null)
                {
                    await UniTask.Delay(5000);
                    ProcessBuyInvoiceLink(data.link);
                }
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
        
        private async void ProcessBuyInvoiceLink(string invoice_link)
        {
            try
            {
                var status = await TelegramWebApp.OpenInvoice(invoice_link);
                if (status)
                {
                    ControllerPopup.ShowToastSuccess("Transaction success");
                    // ControllerResource.Add(TypeResource.ChatPoint, _rewardValue);
                    // ControllerUI.Instance.Spawn(TypeResource.ChatPoint, _position, 20);
                    await FactoryApi.Get<ApiChatInfo>().GetInfo();
                    await FactoryApi.Get<ApiUser>().GetSubscriptions();
                }
                else
                {
                    ControllerPopup.ShowToast(TextId.Shop_FailedPurchased);
                }
            }
            catch
            {
                ControllerPopup.ShowToast(TextId.Shop_FailedPurchased);
            }
        }
    }
}