using BreakInfinity;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ConfirmIdleEarnPanelUnlockSpecial : AConfirmIdleEarnPanelUnlock
    {
        [SerializeField] private TMP_Text txtButtonTitle;
        [SerializeField] private GameObject objArrow;
        [SerializeField] private ConfirmIdleEarnManageCard itemCurrent;
        [SerializeField] private ConfirmIdleEarnManageCard itemNext;

        private BigDouble _cost;
        private string _targetCardId;
        private int _targetCardLevel;

        protected override void OnSetData()
        {
            _targetCardId = Data.conditionData.otherId;
            var targetCard = FactoryApi.Get<ApiUpgrade>().Data.GetCard(_targetCardId);
            if (targetCard != null)
            {
                LoadDescription(Data.conditionData.valueNeed);
                _targetCardLevel = targetCard.current.level;
                SpecialExtensionGame.SetDataCardConfirmIdleEarn(ref itemCurrent, _targetCardId, _targetCardLevel, targetCard.CurrentPointPerHour);
                objArrow.SetActive(false);
                btnUnlock.gameObject.SetActive(false);
                itemNext.gameObject.SetActive(false);

                if (targetCard.current.unlocked)
                {
                    // UnityEngine.Debug.LogError("vao day ko: " + targetCard.current.id);
                    btnUnlock.gameObject.SetActive(true);
                    if (_targetCardLevel == 0)
                    {
                        txtButtonTitle.text = Localization.Get(TextId.Common_Unlock);
                    }
                    else
                    {
                        objArrow.SetActive(true);
                        itemNext.gameObject.SetActive(true);
                        txtButtonTitle.text = Localization.Get(TextId.Common_LevelUp);
                        SpecialExtensionGame.SetDataCardConfirmIdleEarn(ref itemNext, _targetCardId,
                            _targetCardLevel + 1, targetCard.NextPointPerHour);
                    }

                    _cost = targetCard.next.CostParse;
                    txtPrice.text = _cost.ToLetter();
                    txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, _cost);
                }
            }
        }

        private void LoadDescription(int targetConditionValue)
        {
            string cardName = ExtensionEnum.ToIdleEarnName(Data.id);
            var colorHighlight = DBM.Config.visualConfig.GetColorStatus(TypeColor.GREEN_HIGHLIGHT);
            var highLightStr = $"{ExtensionEnum.ToIdleEarnName(_targetCardId)}" +
                               $" {Localization.Get(TextId.Idleearn_DesToLevel)} {targetConditionValue}".
                                   SetHighlightString(colorHighlight);
            // txtDes.text = $"To unlock {cardName} tou must upgrade {highLightStr}";
            txtDes.text = string.Format(Localization.Get(TextId.Idleearn_DesUnlockConditionCard), cardName, highLightStr);
        }

        protected override async void OnUnlock()
        {
            int tempLevel = _targetCardLevel;
            if (string.IsNullOrEmpty(_targetCardId))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Idleearn_NotiNotFoundCard));
                return;
            }
            if (!ControllerResource.IsEnough(Data.typeCurrency, _cost))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughSc));
                return;
            }
            var apiUpgrade = FactoryApi.Get<ApiUpgrade>();
            await apiUpgrade.PostUpgradeIdleEarn(_targetCardId);

            if (tempLevel == 0)
            {
                ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Idleearn_SuccessUnlock),
                    ExtensionEnum.ToIdleEarnName(_targetCardId)));
            }
            else
            {
                ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Toast_SuccessLevelUp));
                // ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Idleearn_SuccessLvUp),
                //     ExtensionEnum.ToIdleEarnName(_targetCardId), tempLevel));
            }

            await FactoryApi.Get<ApiGame>().GetInfo();
        }
    }
}