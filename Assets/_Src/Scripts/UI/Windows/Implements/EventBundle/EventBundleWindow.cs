

using System.Collections.Generic;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class EventBundleWindow : UIWindow
    {
        [SerializeField] private EventBundleScroller scroller;
        [SerializeField] private string eventIdTest;

        private string _eventId;
        private Dictionary<string, ModelApiShopData> _dictShopEventBunle;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            _eventId = this.GetEventData<TypeGameEvent, string>(TypeGameEvent.OpenEventBundle, true);
            if (!string.IsNullOrEmpty(eventIdTest))
                _eventId = eventIdTest;
            
            LoadData();
            ModelApiShopData.OnChanged += OnChangedShop;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            ModelApiShopData.OnChanged -= OnChangedShop;
        }
        
        private void OnChangedShop(ModelApiShopData data)
        {
            LoadData();
        }

        private void LoadData()
        {
            var shopList = FactoryApi.Get<ApiShop>().Data.Shop;
            _dictShopEventBunle = new Dictionary<string, ModelApiShopData>();
            foreach (var shop in shopList)
            {
                if (shop.event_id.Equals(_eventId))
                {
                    _dictShopEventBunle[shop.id] = shop;
                }
            }
            
            var listData = new List<AModelEventBundleCellView>();

            listData.Add(new ModelEventBundleCellViewHeader(_eventId));

            listData.Add(new ModelEventBundleCellViewHeaderTitle(_eventId));
            
            listData.Add(new ModelEventBundleCellViewPackBg(_eventId, _dictShopEventBunle["offer_halloween_background"]));
            
            listData.Add(new ModelEventBundleCellViewPackOfferTimelapse(_eventId, _dictShopEventBunle["offer_halloween_hc"]));
            
            listData.Add(new ModelEventBundleCellViewPackTap(_eventId, _dictShopEventBunle["offer_halloween_tap_eff"]));
            
            scroller.SetData(listData);
        }
    }
}
