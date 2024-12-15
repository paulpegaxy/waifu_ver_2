// Author: ad   -
// Created: 15/09/2024  : : 12:09
// DateUpdate: 15/09/2024

using Game.Model;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class ItemEventBundleTimelapse : AItemEventBundle
    {
        [SerializeField] private ShopItemTimeLapse itemShopTimelapse;
        
        public override void SetData(ModelApiShopData shopData)
        {
            itemShopTimelapse.SetData(shopData);
            var buyCount = shopData.purchased_count;
            var limit = shopData.limit;
            if (buyCount >= limit)
            {
                txtLimit.text = $"{Localization.Get(TextId.Shop_Limit)} {limit}/{limit}";
                objSoldOut.SetActive(true);
                itemShopTimelapse.TurnOnItem(false);
                return;
            }
            
            txtLimit.text = $"{Localization.Get(TextId.Shop_Limit)} {shopData.purchased_count}/{shopData.limit}";
            objSoldOut.SetActive(false);
            itemShopTimelapse.TurnOnItem(true);
            itemShopTimelapse.OnBuySuccess = OnSuccessBuy;
        }

        public override async void OnSuccessBuy()
        {
            await FactoryApi.Get<ApiEvent>().Get();
        }
    }
}