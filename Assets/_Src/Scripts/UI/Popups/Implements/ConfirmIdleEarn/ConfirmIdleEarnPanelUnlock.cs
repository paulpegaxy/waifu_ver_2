using System;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ConfirmIdleEarnPanelUnlock : AConfirmIdleEarnPanelUnlock
    {
        [SerializeField] private ConfirmIdleEarnManageCard itemCurrent;
        [SerializeField] private UIButton btnGoto;

        public static Action OnUnlockIdleEarn;
        
        private BigDouble _cost;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            btnGoto.onClickEvent.AddListener(OnClickGoto);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            btnGoto.onClickEvent.RemoveListener(OnClickGoto);
        }
        
        protected override void OnSetData()
        {
            btnGoto.gameObject.SetActive(!Data.canUnlock);
            btnUnlock.gameObject.SetActive(Data.canUnlock);
            
            string idleName = ExtensionEnum.ToIdleEarnName(Data.id);
            var colorHighlight = DBM.Config.visualConfig.GetColorStatus(TypeColor.GREEN_HIGHLIGHT);
            var highlightString = idleName.SetHighlightString(colorHighlight);
            if (Data.canUnlock)
            {
                txtDes.text = string.Format(Localization.Get(TextId.Idleearn_AskUnlock), highlightString);
                _cost = Data.cost;
                txtPrice.text = Data.cost.ToLetter();
                txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, Data.cost);
            }
            else
            {
                if (Data.conditionData.typeCondition == TypeConditionIdleEarnUnlock.INVITE_FRIEND)
                {
                    int conditionValue = Data.conditionData.valueNeed;
                    LoadMyFriend(conditionValue, colorHighlight);
                }
                else
                    txtDes.text = string.Format(Localization.Get(TextId.Idleearn_AskUnlock), highlightString);
            }

            SpecialExtensionGame.SetDataCardConfirmIdleEarn(ref itemCurrent, Data.id, Data.level+1, Data.profitAfter);
        }

        private async void LoadMyFriend(int conditionValue,Color colorHighlight)
        {
            var dataFriend=await FactoryApi.Get<ApiFriend>().GetFriends();
            var progressStr =
                $" ({Localization.Get(TextId.Idleearn_DesYourFriend)} {dataFriend.data.Count}/{conditionValue})"
                    .SetHighlightString(colorHighlight);
            // txtDes.text = string.Format("To unlock {0}, you must invite {1} friends {2}",
            //     ExtensionEnum.ToIdleEarnName(Data.id), conditionValue, progressStr);
            txtDes.text = string.Format(Localization.Get(TextId.Idleearn_ConditionUnlockInvite),
                ExtensionEnum.ToIdleEarnName(Data.id), conditionValue);
            txtDes.text += progressStr;
        }
        
        protected override async void OnUnlock()
        {
            if (!ControllerResource.IsEnough(Data.typeCurrency, _cost))
            {
                ControllerPopup.ShowToastError(Localization.Get(TextId.Toast_NotEnoughSc));
                return;
            }
            var apiUpgrade = FactoryApi.Get<ApiUpgrade>();
            await apiUpgrade.PostUpgradeIdleEarn(Data.id);
            ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Idleearn_SuccessUnlock),
                ExtensionEnum.ToIdleEarnName(Data.id)));
            OnUnlockIdleEarn?.Invoke();
            await FactoryApi.Get<ApiGame>().GetInfo();
        }
        

        private void OnClickGoto()
        {
            if (Data.conditionData.typeCondition == TypeConditionIdleEarnUnlock.INVITE_FRIEND)
            {
                Signal.Send(StreamId.UI.OpenFriend);
            }
        }
    }
}