using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine.UI;

namespace Game.UI
{
    public class ShopCellViewOfferLevelUp : ShopCellViewOffer
    {
        [SerializeField] private Image imgBackground;

        [SerializeField] private ItemTimer timerDuration;

        // [SerializeField] private TMP_Text txtTime;
        [SerializeField] private TMP_Text textLevel;
        [SerializeField] private TMP_Text txtOriginPrice;
        [SerializeField] private TMP_Text txtCurrPrice;
       
        [SerializeField] private TMP_Text txtSc;
        [SerializeField] private TMP_Text txtHc;

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

                _finalPrice = data.GetFinalPrice();
                txtCurrPrice.text = "$" + _finalPrice.ToDigit();

                btnBuy.gameObject.SetActive(!data.IsReachLimit);
                TurnOnBuyed(data.IsReachLimit);

                if (data.GetItemType() == TypeShopItem.LevelUpOffer1)
                {
                    ProcessDescription(6, GameConsts.MAX_LEVEL_PER_CHAR);
                }
                else
                {
                    ProcessDescription(data.level.from, data.level.to);
                }

                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }

            base.SetData(data);
        }

        private void ProcessDescription(int prevLevel, int nextLevel)
        {
            string prevText = (Localization.Get(TextId.Common_Level).ToLowerCase() + " " + prevLevel)
                .SetHighlightStringGreen2_60FF4B();
            string nextText = (Localization.Get(TextId.Common_Level).ToLowerCase() + " " + nextLevel)
                .SetHighlightStringGreen2_60FF4B();
            textLevel.text = string.Format(Localization.Get(TextId.Shop_DesOfferLevelUp1), prevText, nextText);
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