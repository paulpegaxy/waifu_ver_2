// Author: ad   -
// Created: 28/09/2024  : : 14:09
// DateUpdate: 28/09/2024

using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Newtonsoft.Json;
using Template.Defines;
using UnityEngine;

namespace Game.Extensions
{
    public static class SpecialExtensionShop
    {
        public static async UniTask BuyCheckIn(ModelApiQuestData questData)
        {
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();
                ControllerPopup.SetApiLoading(true);
				
                // await apiShop.BuyWithToken(ShopItemType.CheckIn);
                if (!questData.IsJackpotQuest())
                {
                    ControllerPopup.ShowToastError("Require Jackpot data");
                    return;
                }

                // var index = questData.server_data[0].Split('_')[^1];

                await apiShop.BuyWithTon(questData.server_data[0]);

                ControllerPopup.SetApiLoading(false);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
        
        public static void ShowConfirmPurchaseOnchain(string bundleId,Transform transform)
        {
            var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirmPurchaseBerry.ToString());
            popup.GetComponent<PopupConfirmPurchaseBerry>().SetData(bundleId);
            popup.GetComponent<PopupConfirmPurchaseBerry>().SetItemPosition(transform.position);
            popup.Show();
        }
        
        public static void ShowConfirmPurchaseByHc(string description,ModelApiShopData itemShop)
        {
            var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirmPurchasePrice.ToString());
            popup.GetComponent<PopupConfrmPurchasePrice>().SetData(description, new ModelResource()
            {
                Type = TypeResource.Berry,
                Amount = itemShop.GetFinalPrice()
            }, (data, popupConfirm) =>
            {
                ProcessPurchaseByHc(itemShop, popupConfirm.Hide);
            });
            popup.Show();
        }
        
        public static async void ProcessPurchaseByHc(ModelApiShopData data,Action callBack)
        {
            if (!ControllerResource.IsEnough(TypeResource.Berry, data.GetFinalPrice()))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughHc));
                return;
            }
        
            ControllerPopup.SetApiLoading(true);
            try
            {
                var apiShop=FactoryApi.Get<ApiShop>();
                await apiShop.Buy(data.id);
                await apiShop.Get();
                await FactoryApi.Get<ApiGame>().GetInfo();
                ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Shop_SuccessPurchased));
                callBack?.Invoke();
                ControllerPopup.SetApiLoading(false);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
        
        /// <summary>
        /// Temp process, will optimize based Backend
        /// </summary>
        public static async UniTask RecheckOffer()
        {

            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            // //HARD CHECK
            var currLevel = gameInfo.current_level_girl;
            var apiShop = FactoryApi.Get<ApiShop>();
            ModelApiShopData itemFind = null;
            switch (gameInfo.CurrentCharRank)
            {
                case TypeLeagueCharacter.Char_1:

                    if (currLevel >= 5)
                    {
                        //temp hard check
                        await apiShop.Get();
                        itemFind = apiShop.Data.GetItemByItemType(Template.Defines.TypeShopItem.LevelUpOffer1);
                        if (itemFind != null)
                        {
                            if (ControllerShopOffer.IsExistOffer<ShopOfferLevelUp>())
                            {
                                GameUtils.Log("red", "Exist offer level up");
                                return;
                            }

                            // UnityEngine.Debug.LogError("data offer ne: " + JsonConvert.SerializeObject(itemFind));
                            itemFind.Notification();
                            ControllerShopOffer.CheckPackageLevelUp(itemFind);
                        }
                        else
                        {
                            // UnityEngine.Debug.LogError("Recheck Offer: Not found item offer level up");
                            ControllerShopOffer.CheckExpiredLevelUp();
                        }
                    }

                    break;
                case TypeLeagueCharacter.Char_4:
                case TypeLeagueCharacter.Char_5:

                    await apiShop.Get();
                    itemFind = apiShop.Data.GetItemByItemType(Template.Defines.TypeShopItem.LevelUpOffer2);
                    if (itemFind != null)
                    {
                        if (ControllerShopOffer.IsExistOffer<ShopOfferLevelUp>())
                        {
                            GameUtils.Log("red", "Exist offer level up");
                            return;
                        }

                        itemFind.Notification();
                        ControllerShopOffer.CheckPackageLevelUp(itemFind);
                    }
                    else
                    {
                        // UnityEngine.Debug.LogError("Recheck Offer: Not found item offer level up JISOO");
                        ControllerShopOffer.CheckExpiredLevelUp();
                    }

                    break;
                default:
                    if (ControllerShopOffer.IsExistOffer<ShopOfferLevelUp>())
                    {
                        // UnityEngine.Debug.LogError("Vo check remove tai day");
                        ControllerShopOffer.CheckExpiredLevelUp();
                    }

                    break;
            }
        }

        public static void CheckSaveOnChainTransaction(TypeShopItem type, bool isUpdate)
        {
            var shoppingInfo = FactoryStorage.Get<StorageShopping>();
            var info = shoppingInfo.Get();
            if (type == TypeShopItem.PremiumFieren)
            {
                if (isUpdate)
                {
                    info.lastTimeRequestBuyOfferFreerin =
                        ServiceTime.CurrentUnixTime + GameConsts.DELAY_REQUEST_BUY_ONCHAIN;

                    UnityEngine.Debug.LogError("Save time: " + info.lastTimeRequestBuyOfferFreerin);
                    UnityEngine.Debug.LogError(
                        "Save time: " + TimeSpan.FromSeconds(info.lastTimeRequestBuyOfferFreerin));
                }
                else
                {
                    info.lastTimeRequestBuyOfferFreerin = 0;
                }

                shoppingInfo.Save();
            }
        }

        public static bool VerifyOfferShopCanInterract(ModelApiShopData package)
        {
            if (package.GetItemType() == TypeShopItem.PremiumFieren ||
                package.GetItemType() == TypeShopItem.PremiumFierenTest)
            {
                var shoppingInfo = FactoryStorage.Get<StorageShopping>();
                var info = shoppingInfo.Get();
                if (info.lastTimeRequestBuyOfferFreerin > 0)
                {
                    var timeRemain = info.lastTimeRequestBuyOfferFreerin - ServiceTime.CurrentUnixTime;
                    if (timeRemain > 0)
                    {
                        UnityEngine.Debug.LogError("van con bi delay: " + timeRemain);
                        return false;
                    }
                    else
                    {
                        info.lastTimeRequestBuyOfferFreerin = 0;
                        UnityEngine.Debug.LogError("Over time request buy offer");
                        shoppingInfo.Save();
                    }
                }
            }

            return true;
        }

        public static async UniTask ShowPopupOfferLevelUp()
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            await apiShop.Get();

            var itemFind = apiShop.Data.GetItemsByPack(TypeShopPack.LevelOfferPack).Where(x => !x.IsReachLimit)
                .ToList();

            if (itemFind?.Count > 0)
            {

                var data = itemFind[0];
                var statusAsset = await AnR.CheckAssetIsAvailable<Sprite>(data.GetItemType().ToString());
                if (!statusAsset)
                {
                    ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedGetInfo));
                }

                var keyBg = (AnR.SpriteKey) Enum.Parse(typeof(AnR.SpriteKey), data.GetItemType().ToString());
                await AnR.GetAsync<Sprite>(keyBg);

                var popup = UIPopup.Get(UIId.UIPopupName.PopupOfferLevelUp.ToString());
                popup.GetComponent<PopupOfferLevelUp>().SetData(data);
                UIPopup.AddPopupToQueue(popup);
            }
            else
            {
                // ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedGetInfo));
                // GameUtils.Log("red", "Not found item offer level up");
            }
        }

        public static async UniTask ShowPopupOfferDaySpecial()
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            await apiShop.Get();

            string id = "offer_72hour_timelapse_24h";
            var itemFind = apiShop.Data.GetItemById(id);

            if (itemFind != null)
            {
                if (itemFind.IsReachLimit)
                    return;
                
                var statusAsset = await AnR.CheckAssetIsAvailable<Sprite>(id.SnakeToPascal());
                if (!statusAsset)
                {
                    ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedGetInfo));
                }

                var keyBg = (AnR.SpriteKey) Enum.Parse(typeof(AnR.SpriteKey), id.SnakeToPascal());
                await AnR.GetAsync<Sprite>(keyBg);

                var popup = UIPopup.Get(UIId.UIPopupName.PopupOffer24Hour.ToString());
                popup.GetComponent<PopupOffer24Hour>().SetData(itemFind);
                UIPopup.AddPopupToQueue(popup);
            }
        }
        
        public static async UniTask ShowPopupHalloweenTap()
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            await apiShop.Get();

            var itemFind = apiShop.Data.GetItemById("offer_halloween_tap_eff");

            if (itemFind != null)
            {
                if (itemFind.IsReachLimit)
                    return;
                
                var statusAsset = await AnR.CheckAssetIsAvailable<Sprite>("offer_halloween_tap_eff".SnakeToPascal());
                if (!statusAsset)
                {
                    ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedGetInfo));
                }

                var keyBg = (AnR.SpriteKey) Enum.Parse(typeof(AnR.SpriteKey), "offer_halloween_tap_eff".SnakeToPascal());
                await AnR.GetAsync<Sprite>(keyBg);

                var popup = UIPopup.Get(UIId.UIPopupName.PopupOfferHalloweenTap.ToString());
                popup.GetComponent<PopupOfferHalloweenTapEffect>().SetData(itemFind);
                UIPopup.AddPopupToQueue(popup);
            }
        }

        public static void ShowPopupUpOfferEventBundle()
        {
            var eventBundleOffer = FactoryApi.Get<ApiEvent>().Data.EventBundleOffer;
            if (eventBundleOffer != null)
            {
                var popup = UIPopup.Get(UIId.UIPopupName.PopupOfferEventBundle.ToString());
                popup.GetComponent<PopupOfferEventBundle>().SetData(eventBundleOffer.id);
                UIPopup.AddPopupToQueue(popup);
            }
        }

        public static async UniTask ShowPopupOfferCharFreerin()
        {
            ControllerPopup.SetApiLoading(true);
            var apiShop = FactoryApi.Get<ApiShop>();
            await apiShop.Get();
            var itemFind = apiShop.Data.GetItemsByPack(TypeShopPack.PremiumFieren);

            if (itemFind?.Count > 0)
            {
                ModelApiShopData data = new ModelApiShopData();
#if UNITY_EDITOR && !PRODUCTION_BUILD
                data = itemFind[^1];
#else
            data = itemFind[0];
#endif

                var statusAsset = await AnR.CheckAssetIsAvailable<Sprite>(data.GetPackType().ToString());
                if (!statusAsset)
                {
                    ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedGetInfo));
                }

                var keyBg = (AnR.SpriteKey) Enum.Parse(typeof(AnR.SpriteKey), data.GetPackType().ToString());
                await AnR.GetAsync<Sprite>(keyBg);

                var popup = UIPopup.Get(UIId.UIPopupName.PopupOfferCharFreerin.ToString());
                popup.GetComponent<PopupOfferCharFreerin>().SetData(data);
                UIPopup.AddPopupToQueue(popup);
                ControllerPopup.SetApiLoading(false);
            }
            else
            {
                ControllerPopup.SetApiLoading(false);
                ControllerPopup.ShowToastError(Localization.Get(TextId.Shop_FailedGetInfo));
                GameUtils.Log("red", "Not found item offer level up");
            }
        }
    }
}