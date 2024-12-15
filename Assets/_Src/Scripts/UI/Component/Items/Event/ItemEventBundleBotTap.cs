// Author: ad   -
// Created: 15/09/2024  : : 12:09
// DateUpdate: 15/09/2024

using Game.Model;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class ItemEventBundleBotTap : AItemEventBundle
    {
        [SerializeField] private ShopCellViewBotTap itemShopBotTap;

        public override void SetData(ModelApiShopData shopData)
        {
            itemShopBotTap.SetData(new ModelShopCellViewContentBotTap()
            {
                BotTapData = shopData
            });
            if (SpecialExtensionGame.IsAutoBotPurchased())
            {
                txtLimit.text = $"{Localization.Get(TextId.Shop_Limit)} {shopData.limit}/{shopData.limit}";
                objSoldOut.SetActive(true);
                itemShopBotTap.TurnOnItem(false);
                return;
            }
            
            txtLimit.text = $"{Localization.Get(TextId.Shop_Limit)} {shopData.purchased_count}/{shopData.limit}";
            objSoldOut.SetActive(false);
            itemShopBotTap.TurnOnItem(true);
            itemShopBotTap.OnBuySuccess = OnSuccessBuy;
        }

        public override async void OnSuccessBuy()
        {
            await FactoryApi.Get<ApiEvent>().Get();
        }
    }
}