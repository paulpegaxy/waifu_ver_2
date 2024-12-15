using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ShopCellViewOfferCharFreerin : ShopCellViewOffer
    {
        [SerializeField] private Image imgBackground;
        [SerializeField] private ItemTimer timerDuration;
        [SerializeField] private TMP_Text txtDes;
        [SerializeField] private TMP_Text txtPrice;
        [SerializeField] private TMP_Text txtUnlock;

        [SerializeField] private UIButton btnBuy;
        [SerializeField] private GameObject objSoldOut;

        private float _finalPrice;
        private int _charId;
        
        private void OnEnable()
        {
            btnBuy.onClickEvent.AddListener(OnBuy);
        }

        private void OnDisable()
        {
            btnBuy.onClickEvent.RemoveListener(OnBuy);
            objSoldOut.SetActive(false);
        }
        
        protected override void SetData(ModelApiShopData data)
        {
            try
            {
                this.ShowProcessing();
                // var keyBg = (AnR.SpriteKey)Enum.Parse(typeof(AnR.SpriteKey), data.GetItemType().ToString());
                imgBackground.LoadSpriteAutoParseAsync(data.pack);
                
                var timeRemain = data.end_time.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
                timerDuration.SetDuration(timeRemain);

                _finalPrice = data.GetFinalPrice();
                
                txtPrice.text = "$" + _finalPrice.ToDigit();

                btnBuy.gameObject.SetActive(!data.IsReachLimit);
                objSoldOut.SetActive(data.IsReachLimit);
                
                LoadCharInfo(data.items[0].id);
                ProcessDescription(data);
                
                this.HideProcessing();
            }
            catch (Exception e)
            {
                transform.DOScale(1, 0);
                e.ShowError();
            }

            base.SetData(data);
        }

        private void LoadCharInfo(string idReward)
        {
            txtUnlock.text = Localization.Get(TextId.Common_Unlock);
            _charId = int.Parse(idReward);
            var charData = DBM.Config.charPremiumConfig.GetCharData(_charId);
            if (charData != null)
            {
                txtUnlock.text += " " + charData.name;
            }
        }

        private void ProcessDescription(ModelApiShopData data)
        {
            string des = "";
            if (data.all_user_purchased_count != null)
            {
                int remain = data.limit_all_user - (int)data.all_user_purchased_count;
                if (remain > 0)
                {
                    des = $"{data.all_user_purchased_count}/{data.limit_all_user}".SetHighlightStringGreen2_60FF4B();
                }
                else
                {
                    des = $"{data.all_user_purchased_count}/{data.limit_all_user}".SetHighlightStringRed();
                }
            }

            txtDes.text = string.Format(Localization.Get(TextId.Shop_FreerinStatus), des);
        }

        private async void OnBuy()
        {
            this.ShowProcessing();
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();
#if UNITY_EDITOR
                var status = await ProcessBuyTest();
                if (!status)
                {
                    this.HideProcessing();
                    return;
                }
#else
                // await apiShop.BuyWithTon(_data.id,typeShopItem: TypeShopItem.PremiumFieren);
#endif
                FactoryApi.Get<ApiGame>().GetInfo().Forget();
                await apiShop.Get();

                ControllerPopup.ShowToast(Localization.Get(TextId.Shop_SuccessPurchased));
                btnBuy.gameObject.SetActive(false);
                objSoldOut.SetActive(true);
                base.InvokeBuySuccess();
                this.PostEvent(TypeGameEvent.SuccessBuyOffer, true);
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
                ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedPurchased));
            }
        }

        private async UniTask<bool> ProcessBuyTest()
        {
            if (!ControllerResource.IsEnough(TypeResource.Berry, _finalPrice))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
                return false;
            }

            await FactoryApi.Get<ApiShop>().Buy(TypeShopItem.PremiumFierenTest);
            return true;
        }
    }
}