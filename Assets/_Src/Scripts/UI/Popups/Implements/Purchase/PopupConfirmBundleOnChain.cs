using System;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
    public abstract class PopupConfirmBundleOnChain : PopupConfirmBundle
    {
        protected override async void OnBuy()
        {
            this.ShowProcessing();
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();
                var data = await apiShop.BuyWithStar(_data.id);

                OnBuySuccess(data);
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
                // ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedPurchased));
            }
        }
    }
}