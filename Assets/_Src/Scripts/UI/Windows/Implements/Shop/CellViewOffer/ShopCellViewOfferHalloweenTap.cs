// Author: ad   -
// Created: 04/11/2024  : : 22:11
// DateUpdate: 04/11/2024

using Cysharp.Threading.Tasks;

namespace Game.UI
{
    public class ShopCellViewOfferHalloweenTap : AShopCellViewOfferPurchase
    {
        protected override void OnSetData()
        {
            
        }

        protected override async UniTask OnCompetePurchase()
        {
            var config = DBM.Config.tapEffectConfig.GetTapEffect(Data.items[0].id);
            SpecialExtensionGame.ProcessSelectTapEffect(config.id);
        }
    }
}