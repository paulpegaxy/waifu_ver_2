using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.UI
{
    public class ButtonBundleShop24HourOffer : AButtonBundleShop
    {
        [SerializeField] private ItemTimer timerDuration;
        private bool _isInit;
        
        protected override bool OnValidateEvent(ModelApiShop data)
        {
            return true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!_isInit)
            {
                _isInit = true;
                return;
            }

            var apiShop = FactoryApi.Get<ApiShop>();
            if (apiShop.Data.Shop == null)
                return;

            Refresh(FactoryApi.Get<ApiShop>().Data.GetItemById("offer_72hour_timelapse_24h"));
            ModelApiShopData.OnChanged += OnShopChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ModelApiShopData.OnChanged -= OnShopChanged;
        }

        private void OnShopChanged(ModelApiShopData data)
        {
            Refresh(data);
        }

        private void Refresh(ModelApiShopData data)
        {
            if (data==null)
                return;
            var timeRemain = data.end_time.ToUnixTimeSeconds() - ServiceTime.CurrentUnixTime;
            timerDuration.SetDuration(timeRemain);
        }

        protected override void OnClick()
        {
            // SpecialExtensionShop.ShowPopupOfferDaySpecial().Forget();
        }
    }
}