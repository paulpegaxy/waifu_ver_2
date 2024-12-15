// Author:    -    ad
// Created: 28/07/2024  : : 5:22 PM
// DateUpdate: 28/07/2024

using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BoosterItemChargeStamina : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtNum;
        [SerializeField] private TMP_Text txtRefreshTime;
        [SerializeField] private UIButton btnCharge;
        [SerializeField] private Image imgIcon;
        [SerializeField] private GameObject objHolderTime;
        [SerializeField] private GameObject objArrow;

        private ModelApiUpgradeInfo _upgradeData;

        private void Awake()
        {
            btnCharge.onClickEvent.AddListener(OnClickChargeStamina);
        }

        private void OnDestroy()
        {
            btnCharge.onClickEvent.RemoveListener(OnClickChargeStamina);
        }

        public void LoadData()
        {
            _upgradeData = FactoryApi.Get<ApiUpgrade>().Data;
            objArrow.SetActive(_upgradeData.current.charge_stamina > 0);
            var colorNum = txtNum.color;
            txtNum.color = new Color(colorNum.r, colorNum.g, colorNum.b,
                _upgradeData.current.charge_stamina > 0 ? 1 : 0.5f);
            if (_upgradeData.current.charge_stamina > 0)
                txtNum.text = $"{_upgradeData.current.charge_stamina}/{_upgradeData.current.charge_stamina_max} " +
                              $"{Localization.Get(TextId.Common_Available)}";
            else
            {

                txtNum.text = Localization.Get(TextId.Common_NoAvailable);
            }

            // imgIcon.sprite = ControllerSprite.Instance.GetBoosterIcon(TypeBooster.ENERGY_RESTORE);
            imgIcon.LoadSpriteAutoParseAsync("booster_" + (int)TypeBooster.ENERGY_RESTORE);
            int timeDisplay = _upgradeData.next.next_charge_stamina_wait_time;
            if (timeDisplay > 0)
            {
                objHolderTime.SetActive(true);
                if (timeDisplay < 60)
                    txtRefreshTime.text = "1";
                else
                    txtRefreshTime.text = (timeDisplay / 60).ToString();
                txtRefreshTime.text += $" {Localization.Get(TextId.Toast_NotiMinuteLeft)}";
            }
            else
            {
                objHolderTime.SetActive(false);
            }

        }

        private void OnClickChargeStamina()
        {
            var currStamina = ControllerResource.Get(TypeResource.ExpWaifu).Amount;
            if (_upgradeData.current.charge_stamina <= 0)
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Booster_NoAvailableRestore));
                return;
            }

            var maxStamina = FactoryApi.Get<ApiGame>().Data.Info.stamina_max;
            if (currStamina >= maxStamina)
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Booster_NotiCannotRestore));
                return;
            }

            int timeDisplay = _upgradeData.next.next_charge_stamina_wait_time;
            if (timeDisplay > 0)
            {
                ControllerPopup.ShowToastError(string.Format(Localization.Get(TextId.Booster_NotiWaitRecharge),
                    txtRefreshTime.text));
                return;
            }

            this.ShowPopup(UIId.UIPopupName.PopupConfirmBoosterRestoreEnergy);
        }
    }
}