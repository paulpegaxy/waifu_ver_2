using System;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class MainHeaderLeftGroupButton : AItemCheckStartGame
    {
        [SerializeField] private UIButton btnBundleFreerin;
        private bool _isInitiated;
        
        protected override void OnEnabled()
        {
            // var data = this.GetEventData<TypeGameEvent, bool>(TypeGameEvent.SuccessBuyOffer, true);
            // if (data)
            // {
            //     CheckBundleCharFreerin();
            // }
            // btnBundleFreerin.onClickEvent.AddListener(OnClickFreeRin);    
            
            if (_isInitiated == false)
            {
                _isInitiated = true;
                return;
            }
            
            // CheckBundleCharFreerin();
            ModelApiShopData.OnChanged += OnShopChanged;
        }

        protected override void OnDisabled()
        {
            // btnBundleFreerin.onClickEvent.RemoveListener(OnClickFreeRin);
            ModelApiShopData.OnChanged -= OnShopChanged;
        }
        
        protected override void OnInit()
        {
            // CheckBundleCharFreerin();
        }

        private void OnShopChanged(ModelApiShopData data)
        {
            // CheckBundleCharFreerin();
        }

//         private void CheckBundleCharFreerin()
//         {
//             var shopData = FactoryApi.Get<ApiShop>().Data;
// #if UNITY_EDITOR && !PRODUCTION_BUILD
//             var itemFind = shopData.GetItemByItemType(TypeShopItem.PremiumFierenTest);
//             if (itemFind != null)
//             {
//                 // btnBundleFreerin.gameObject.SetActive(!itemFind.IsReachLimit);
//                                                       // && !itemFind.IsSoldOut);
//                 btnBundleFreerin.gameObject.SetActive(!itemFind.IsReachLimit && !itemFind.IsSoldOut &&
//                                                       SpecialExtensionShop.VerifyOfferShopCanInterract(itemFind));
//             }
// #else
//             var itemFind = shopData.GetItemByItemType(TypeShopItem.PremiumFieren);
//             if (itemFind != null)
//             {
//                 btnBundleFreerin.gameObject.SetActive(!itemFind.IsReachLimit && !itemFind.IsSoldOut &&
//                                                       SpecialExtensionShop.VerifyOfferShopCanInterract(itemFind));
//                 return;
//             }
// #endif
//
//             btnBundleFreerin.gameObject.SetActive(false);
//         }

        private void OnClickFreeRin()
        {
            SpecialExtensionShop.ShowPopupOfferCharFreerin();
        }
    }
}