using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Runtime;
using Game.Model;
using BreakInfinity;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Template.Defines;

namespace Game.UI
{
    public class PopupIdleEarning : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtPoint;
        [SerializeField] private TMP_Text txtPointPremium;
        [SerializeField] private TMP_Text textPrice;
        [SerializeField] private UIButton buttonFree;
        [SerializeField] private UIButton btnAds;
        [SerializeField] private UIButton buttonPremium;

        private ModelApiGameIdleEarning _data;
        
        private void Start()
        {
            buttonFree.onClickEvent.AddListener(OnFree);
            buttonPremium.onClickEvent.AddListener(OnPremium);
            btnAds.onClickEvent.AddListener(OnAds);
        }

        private void OnDestroy()
        {
            buttonFree.onClickEvent.RemoveListener(OnFree);
            buttonPremium.onClickEvent.RemoveListener(OnPremium);
            btnAds.onClickEvent.RemoveListener(OnAds);
        }

        private async void OnFree()
        {
            var apiGame = FactoryApi.Get<ApiGame>();
            await apiGame.IdleClaimFree();
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Toast_IdleEarnSuccess));

            GetComponent<UIPopup>().Hide();
        }

        private void OnAds()
        {
            ControllerPopup.ShowToastComingSoon();
        }

        private void ShowConfirmPremium()
        {
            ControllerPopup.ShowConfirmPurchase(Localization.Get(TextId.Confirm_IdleEarnHc),
                new ModelResource()
                {
                    Amount = BigDouble.Parse(_data.premium.price.ToString()),
                    Type = TypeResource.Berry
                }, (item,popup) => ProcessClaimPremium(popup));
        }

        private async void ProcessClaimPremium(UIPopup popup)
        {
            this.ShowProcessing();
            var api = FactoryApi.Get<ApiGame>();
            try
            {
                popup.Hide();
                GetComponent<UIPopup>().Hide();
                await api.IdleClaimPremium();
                this.HideProcessing();
                ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Toast_IdleEarnSuccess));
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            
     
        }
        
        
        private void ProcessTextPrice(BigDouble priceRequire)
        {
            var currPoint = ControllerResource.Get(TypeResource.Berry).Amount;
            var visualConfig = DBM.Config.visualConfig;
            textPrice.color = currPoint < priceRequire
                ? visualConfig.GetColorStatus(TypeColor.RED)
                : visualConfig.GetColorStatus(TypeColor.WHITE);
        }

        private void OnPremium()
        {
            if (!ControllerResource.IsEnough(TypeResource.Berry, _data.premium.price))
            {
                string btnOkName = Localization.Get(TextId.Common_GoTo) + " " + Localization.Get(TextId.Common_Shop);
                // EventManager.EmitEvent(TypeGameEvent.ShopOpenFromIdleEarning, true);
                ControllerPopup.ShowConfirm(Localization.Get(TextId.Confirm_IdleEarnGotoShop),
                    btnOkName, Localization.Get(TextId.Common_Cancel), (popup) =>
                    {
                        GetComponent<UIPopup>().Hide();
                        this.PostEvent(TypeGameEvent.ShopOpenFromIdleEarning, true);
                        Signal.Send(StreamId.UI.OpenShop);
                        popup.Hide();
                    });
                return;
            }

            ShowConfirmPremium();
        }

        public void SetData(ModelApiGameIdleEarning data)
        {
            txtPoint.text = data.free.PointParse.ToLetter();
            txtPointPremium.text = data.premium.PointParse.ToLetter();
            textPrice.text = data.premium.price.ToString();
            ProcessTextPrice(data.premium.price);
            _data = data;
        }
    }
}