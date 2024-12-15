using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Model;
using Game.Runtime;
using Template.Runtime;

namespace Game.UI
{
    public class PopupFirstPurchase : PopupConfirmBundleOnChain
    {
        [SerializeField] private ItemCurrencyColor currencyPal;
        [SerializeField] private ItemCurrencyColor currencyGold;
        [SerializeField] private TMP_Text textValuable;
        [SerializeField] private TMP_Text textPrice;

        protected override void OnBuySuccess(ModelApiShopBuy data)
        {
            var apiUser = FactoryApi.Get<ApiUser>();
            apiUser.Data.Shop.is_first_purchase = true;
            apiUser.Data.Notification();

            foreach (var item in data.bundle_data.items)
            {
                ControllerResource.Add(item.IdResource, item.QuantityParse);
                ControllerUI.Instance.Spawn(item.IdResource, transform.position, 20);
            }

            GetComponent<UIPopup>().Hide();
        }

        protected override void OnData(ModelApiShopData data)
        {
            currencyGold.SetAmount(data.items[0].QuantityParse);
            currencyPal.SetAmount(data.items[1].QuantityParse);

            textValuable.text = $"{1000}%";
            textPrice.text = $"{data.ton_price} TON";
        }
    }
}