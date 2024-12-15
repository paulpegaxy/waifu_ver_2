// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemButtonGirlCardOverlayUndressPremiumCooldown : AItemButtonGirlCardOverlayPremium
    {
        [SerializeField] private ItemTimerSlider slider;
        [SerializeField] private TMP_Text txtPrice;
        [SerializeField] private ItemTimer txtTime;

        private ModelApiUpgradePremiumChar _charData;
        
        protected override void OnSetData()
        {
            _charData = FactoryApi.Get<ApiUpgrade>().Data.GetPremiumChar(Data.girlId);
            txtPrice.text = _charData.next.CostParse.ToLetter();
            txtTime.SetDuration(_charData.GetCooldownUndress(), Reload);
            slider.SetDuration(_charData.GetCooldownUndress(), _charData.time_to_next);
        }

        private async void Reload()
        {
            await FactoryApi.Get<ApiUpgrade>().Get();
        }

        protected override void OnClick()
        {
            var popup=this.ShowPopup<PopupConfirmPurchaseSkipTime>(UIId.UIPopupName.PopupConfirmPurchaseSkipTime);
            popup.Show(_charData);
        }
    }
}