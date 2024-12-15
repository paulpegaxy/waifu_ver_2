using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Sirenix.Utilities;
using Template.Defines;
using Unity.VisualScripting;

namespace Game.UI
{
    public class ItemPackage : AItemCheckStartGame
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private UIToggle togglePackage;

        private Dictionary<MainWindowAction, ButtonMainWindowAction> _actionButtons = new();
        private Dictionary<MainWindowAction, bool> _actionVisible = new();

        private IButtonItemPackage[] _arrBtnItemPackage;

        protected override void Awake()
        {
            base.Awake();
            _arrBtnItemPackage = gridLayoutGroup.GetComponentsInChildren<IButtonItemPackage>(true);
            var buttons = GetComponentsInChildren<ButtonMainWindowAction>(true);
            foreach (var button in buttons)
            {
                _actionButtons[button.Action] = button;
                _actionVisible[button.Action] = false;
            }
        }

        protected override void OnInit()
        {
            ControllerShopOffer.Init();
            Refresh24HourOffer();
            _arrBtnItemPackage.ForEach(x => x.Init());

            OnEventChanged(FactoryApi.Get<ApiEvent>().Data);

            if (!ControllerShopOffer.IsExpired())
            {
                OnShopOffer("");
            }

            OnPackage(togglePackage.isOn);
        }

        protected override void OnEnabled()
        {
            ModelApiUser.OnChanged += OnUserChanged;
            ModelApiEvent.OnChanged += OnEventChanged;
            ModelApiShopData.OnChanged += OnShopChanged;
            ShopOffer.OnOffer += OnShopOffer;
            ShopOffer.OnExpired += OnShopOfferExpired;
            togglePackage.OnValueChangedCallback.AddListener(OnPackage);
        }

        protected override void OnDisabled()
        {
            ModelApiUser.OnChanged -= OnUserChanged;
            ModelApiEvent.OnChanged -= OnEventChanged;
            ModelApiShopData.OnChanged -= OnShopChanged;
            ShopOffer.OnOffer -= OnShopOffer;
            ShopOffer.OnExpired -= OnShopOfferExpired;
            togglePackage.OnValueChangedCallback.RemoveListener(OnPackage);
        }

        private void OnUserChanged(ModelApiUser data)
        {
            // SetItem(MainWindowAction.FirstPurchase, !data.Shop.is_first_purchase);
            if (data.User.IsHavePrivatePartner("yggplay"))
            {
                SetItem(MainWindowAction.PartnerYggPlay, true);
            }

        }

        private void OnShopChanged(ModelApiShopData data)
        {
            Refresh24HourOffer();
        }

        private void OnEventChanged(ModelApiEvent data)
        {
            if (data.events.Count <= 0)
                return;

            SetItem(MainWindowAction.EventMeetYuki, !data.IsEndEventMeetYuki());

            var partnerMerge = data.GetEventByType(MainWindowAction.PartnerMergePal);
            SetItem(MainWindowAction.PartnerMergePal, partnerMerge != null);

            var partnerWaifuPride = data.GetEventByType(MainWindowAction.PartnerWaifuPride);
            SetItem(MainWindowAction.PartnerWaifuPride, partnerWaifuPride != null);

            var partnerEtaku = data.GetEventByType(MainWindowAction.PartnerEtaku);
            SetItem(MainWindowAction.PartnerEtaku, partnerEtaku != null);

            var eventYgg = data.GetEventByType(MainWindowAction.PartnerYggPlay);
            if (eventYgg != null)
            {
                var apiUser = FactoryApi.Get<ApiUser>();
                if (apiUser.Data.User.IsHavePrivatePartner(eventYgg.tag_private[0]))
                {
                    SetItem(MainWindowAction.PartnerYggPlay, true);
                }
                else
                {
                    SetItem(MainWindowAction.PartnerYggPlay, false);
                }
            }

            if (data.Jackpot != null)
            {
                SetItem(MainWindowAction.Jackpot, ServiceTime.CurrentUnixTime >= data.Jackpot.open_date);
            }
        }

        private void Refresh24HourOffer()
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            var itemFind = apiShop.Data.GetItemById("offer_72hour_timelapse_24h");
            if (itemFind?.IsReachLimit == false)
            {
                SetItem(MainWindowAction.Offer24Hour, true);
            }
            else
            {
                SetItem(MainWindowAction.Offer24Hour, false);
            }
        }

        private void OnShopOffer(string id)
        {
            // bool isActive = FactoryApi.Get<ApiShop>().Data.GetItemsByPack(TypeShopPack.LevelOfferPack)
            //     .Any(x => !x.IsReachLimit);
            //
            // SetItem(MainWindowAction.LimitedOffer, isActive);

            SetItem(MainWindowAction.LimitedOffer, true);
        }

        private void OnShopOfferExpired(string id)
        {
            if (ControllerShopOffer.IsExpired())
            {
                SetItem(MainWindowAction.LimitedOffer, false);
            }
        }

        private void OnPackage(bool isOn)
        {
            foreach (var button in _actionButtons.Values)
            {
                button.gameObject.SetActive(isOn && _actionVisible[button.Action]);
            }

            foreach (var button in _actionButtons.Values)
            {
                if (_actionVisible[button.Action])
                {
                    button.gameObject.SetActive(true);
                    break;
                }
            }

            Refresh();
        }

        private void Refresh()
        {
            var count = 0;
            foreach (var button in _actionButtons.Values)
            {
                if (button.gameObject.activeSelf && _actionVisible[button.Action])
                {
                    count++;
                }
            }

            var visible = _actionVisible.Where(x => x.Value).Count();
            togglePackage.gameObject.SetActive(visible > 1);

            gridLayoutGroup.gameObject.SetActive(count > 0);
            gridLayoutGroup.constraintCount = Mathf.Min(count, 3);
            gridLayoutGroup.padding.bottom = togglePackage.gameObject.activeSelf ? 70 : 20;
        }

        public void SetItem(MainWindowAction action, bool visible)
        {
            var button = _actionButtons[action];
            if (button == null || _actionVisible[action] == visible) return;

            _actionVisible[action] = visible;
            button.gameObject.SetActive(togglePackage.isOn && visible);

            togglePackage.SetIsOn(false);
            togglePackage.SetIsOn(true);
        }
    }
}