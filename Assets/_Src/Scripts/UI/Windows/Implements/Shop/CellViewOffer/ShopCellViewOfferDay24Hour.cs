using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ShopCellViewOfferDay24Hour : ShopCellViewOffer
    {
        [SerializeField] private Image imgBackground;
        
        [SerializeField] private ItemTimer timerDuration;

        [SerializeField] private TMP_Text txtOriginPrice;
        [SerializeField] private TMP_Text txtCurrPrice;

        [SerializeField] private TMP_Text txtSc;
        [SerializeField] private TMP_Text txtHc;
        [SerializeField] private TMP_Text txtDesTimelapse;
        [SerializeField] private TMP_Text txtTimelapse;
        [SerializeField] private Image imgTimelapse;


        [SerializeField] private UIButton btnBuy;

        [SerializeField] private GameObject[] arrObjBuyed;


        private float _finalPrice;

        private void OnEnable()
        {
            btnBuy.onClickEvent.AddListener(OnBuy);
        }

        private void OnDisable()
        {
            btnBuy.onClickEvent.RemoveListener(OnBuy);
            TurnOnBuyed(false);
        }

        protected override async void SetData(ModelApiShopData data)
        {
            try
            {
                this.ShowProcessing();

                imgBackground.LoadSpriteAutoParseAsync(data.id);

                var timeRemain = data.end_time.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
                timerDuration.SetDuration(timeRemain);

                txtOriginPrice.text = "$" + data.price;

                txtHc.text = data.items[0].ValueParse.ToLetter();
                txtSc.text = data.items[^1].ValueParse.ToLetter();
                
                ProcessTimelapseContent(data);

                // string highlightValueStr = timelapseHour.SetHighlightStringOrange();
                // txtDesTimelapse.text = string.Format(Localization.Get(TextId.Shop_DesTimelapse), highlightValueStr.Replace("h", ""));

                _finalPrice = data.GetFinalPrice();
                txtCurrPrice.text = "$" + _finalPrice.ToDigit();

                btnBuy.gameObject.SetActive(!data.IsReachLimit);
                TurnOnBuyed(data.IsReachLimit);

                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }

            base.SetData(data);
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
            
            string highlightValueStr = strTime.Replace("H", "").SetHighlightStringOrange();
            txtDesTimelapse.text = string.Format(Localization.Get(TextId.Shop_DesTimelapse), highlightValueStr.Replace("h", ""));
        }

        private void TurnOnBuyed(bool isOn)
        {
            for (int i = 0; i < arrObjBuyed.Length; i++)
            {
                arrObjBuyed[i].SetActive(isOn);
            }
        }

        private async void OnBuy()
        {
            this.ShowProcessing();
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();

                await apiShop.BuyWithTon(Data.id);

                await FactoryApi.Get<ApiGame>().GetInfo();
                await apiShop.Get();
                ControllerPopup.ShowToast(Localization.Get(TextId.Shop_SuccessPurchased));
                btnBuy.gameObject.SetActive(false);
                TurnOnBuyed(true);
                base.InvokeBuySuccess();
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