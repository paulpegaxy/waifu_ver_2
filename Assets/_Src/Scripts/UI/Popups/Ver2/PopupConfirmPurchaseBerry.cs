 using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;
using UnityEngine.UI;

namespace Game.UI
{
    public class PopupConfirmPurchaseBerry : PopupConfirmBundleOnChain
    {
        [SerializeField] private TMP_Text txtCurrency;
        [SerializeField] private TMP_Text txtPrice;
        // [SerializeField] private TMP_Text txtBonusValue;
        // [SerializeField] private TMP_Text txtItemName;
        // [SerializeField] private TMP_Text textPrice;
        // [SerializeField] private GameObject objBonusTag;

        // [SerializeField] private Image imgIconShopItem;
        // [SerializeField] private Image imgIconCurrency;

        private Vector3 _position;
        private int _rewardValue;

        protected override void OnBuySuccess(ModelApiShopBuy data)
        {
            
        }

        protected override void OnBuySuccess(ModelApiShopBuyWithStar data)
        {
            // foreach (var item in data.bundle_data.items)
            // {
            //     ControllerResource.Add(item.IdResource, item.ValueParse);
            //     ControllerUI.Instance.Spawn(item.IdResource, _position, 20);
            // }
            //
            // var apiUser = FactoryApi.Get<ApiUser>();
            // apiUser.Data.Shop.received_bonus.Add(data.bundle_data.id);
            //
            // var apiShop = FactoryApi.Get<ApiShop>();
            // apiShop.Data.Notification();
            
            ProcessBuyInvoiceLink(data);
        }

        private async void ProcessBuyInvoiceLink(ModelApiShopBuyWithStar data)
        {
            this.ShowProcessing();
            try
            {
                var status = await TelegramWebApp.OpenInvoice(data.invoice_link);
                if (status)
                {
                    ControllerResource.Add(TypeResource.ChatPoint, _rewardValue);
                    ControllerUI.Instance.Spawn(TypeResource.ChatPoint, _position, 20);
                    // var rewards = _data.rewards.FindAll(x => x.id != Defines.ResourceType.Boost);
                    // for (var i = 0; i < rewards.Count; i++)
                    // {
                    //     var reward = rewards[i];
                    //     ControllerResource.Add(reward.id, reward.Quantity);
                    //     ControllerUI.Instance.Spawn(reward.id, itemSubscriptions[i].transform.position, 20);
                    // }

                    // _data.telegram_subscription = true;
                    // apiUser.Data.Notification();
                    GetComponent<UIPopup>().Hide();
                }
                else
                {
                    ControllerPopup.ShowToast(TextId.Shop_FailedPurchased);
                }
                this.HideProcessing();
            }
            catch
            {
                this.HideProcessing();
                ControllerPopup.ShowToast(TextId.Shop_FailedPurchased);
            }
        }

        protected override void OnData(ModelApiShopData data)
        {
            // textPrice.text = $"{data.ton_price}";
            _rewardValue = int.Parse(data.items[0].value);
            if (data.bonus_items.Count > 0)
            {
                _rewardValue += int.Parse(data.bonus_items[0].value);
            }

            txtCurrency.text = $"+{_rewardValue}";
            txtPrice.text = data.GetTokenStar.price.ToString();

            // objBonusTag.SetActive(!data.IsReceivedBonus(data.id));
        }

        public void SetItemPosition(Vector3 position)
        {
            _position = position;
        }
    }
}