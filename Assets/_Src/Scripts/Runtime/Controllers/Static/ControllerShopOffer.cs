using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.UI;
using Template.Defines;

namespace Game.Runtime
{
    public static class ControllerShopOffer
    {
        public static readonly List<ShopOffer> Offers = new();

        public static void Init()
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            Offers.Clear();

            var levelUpOffer = apiShop.Data.GetItemsByPack(TypeShopPack.LevelOfferPack);
            foreach (var item in levelUpOffer)
            {
                var offer = new ShopOfferLevelUp();
                offer.Init(item.id);
                Offers.Add(offer);
            }

            // var specialOffer = apiShop.Data.GetItemsByPack(TypeShopPack.SpecialOfferPack);
            // foreach (var item in specialOffer)
            // {
            //     var offer = new ShopOfferSpecial();
            //     offer.Init(item.id);
            //     Offers.Add(offer);
            // }
            
            var halloweenOffer = apiShop.Data.GetItemById("offer_halloween_tap_eff");
            if (halloweenOffer?.IsReachLimit==false)
            {
                var offer = new ShopOfferHalloween();
                offer.Init(halloweenOffer.id);
                Offers.Add(offer);
            }
        }

        public static void CheckExpiredLevelUp()
        {
            var itemFind = Offers.Find(x => x.Type == ShopOfferType.LevelUp);
            // UnityEngine.Debug.LogError("CheckExpiredLevelUp");
            if (itemFind != null)
            {
                // UnityEngine.Debug.LogError("Vo day Check en");
                Offers.Remove(itemFind);
                itemFind.Expired();
            }
        }


        public static void CheckPackageLevelUp(ModelApiShopData item)
        {
            if (Offers.Exists(x => x.Type == ShopOfferType.LevelUp))
            {
                return;
            }

            // var apiShop = FactoryApi.Get<ApiShop>();
            // var item = apiShop.Data.GetItemById(TypeShopItem.LevelUpOffer1);
            if (item != null)
            {
                var offer = new ShopOfferLevelUp();
                offer.Init(item.id);
                Offers.Add(offer);
                SpecialExtensionShop.ShowPopupOfferLevelUp().Forget();
            }
        }

        public static bool IsExistOffer<T>()
        {
            return Offers.Exists(x => x is T);
        }

        public static void Update(float deltaTime)
        {
            foreach (var offer in Offers)
            {
                offer.Update(deltaTime);
            }
        }

        public static void ReCheckAllOffer()
        {
            foreach (var offer in Offers)
            {
                offer.Refresh();
            }
        }

        public static bool IsExpired()
        {
            foreach (var offer in Offers)
            {
                if (!offer.IsExpired())
                {
                    return false;
                }
            }

            return true;
        }

        public static List<ShopOffer> GetOffer<T>()
        {
            return Offers.FindAll(x => x is T);
        }
    }
}