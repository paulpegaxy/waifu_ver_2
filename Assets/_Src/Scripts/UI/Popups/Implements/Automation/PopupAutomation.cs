using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class PopupAutomation : PopupConfirmBundle
    {
        [SerializeField] private TMP_Text textSaleOff;
        [SerializeField] private TMP_Text textPriceOriginal;
        [SerializeField] private TMP_Text textPriceDiscounted;

        protected override async void OnBuySuccess(ModelApiShopBuy data)
        {
            var apiUser = FactoryApi.Get<ApiUser>();
            apiUser.Data.Notification();

            var apiGame = FactoryApi.Get<ApiGame>();
            await apiGame.GetInfo();

            // ControllerAutomation.Start();
            GetComponent<UIPopup>().Hide();
        }

        protected override void OnData(ModelApiShopData data)
        {
            textSaleOff.text = $"{data.sale_off_percent * 100}%";
            textPriceOriginal.text = data.price.ToString();
            textPriceDiscounted.text = $"{Mathf.FloorToInt(data.price * data.sale_off_percent)}";

            _data = data;
        }
    }
}