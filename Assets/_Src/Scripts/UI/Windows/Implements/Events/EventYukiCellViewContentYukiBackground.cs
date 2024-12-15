using System;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EventYukiCellViewContentYukiBackground : ESCellView<AModelEventYukiCellView>
    {
        [SerializeField] private TMP_Text txtStatus;
        [SerializeField] private TMP_Text txtDes;
        [SerializeField] private GameObject objDes;
        [SerializeField] private GameObject objClaimed;
        [SerializeField] private UIButton btnClaim;
        [SerializeField] private Image imgBtnClaim;
        [SerializeField] private GameObject objCanClaim;

        private void OnEnable()
        {
            btnClaim.onClickEvent.AddListener(OnClaimBackground);
        }

        private void OnDisable()
        {
            btnClaim.onClickEvent.RemoveListener(OnClaimBackground);
        }

        public override void SetData(AModelEventYukiCellView data)
        {
            if (data is ModelEventYukiCellViewContentYukiBackground modelData)
            {
                // Do something
                var apiUpgrade = FactoryApi.Get<ApiUpgrade>().Data;
                if (apiUpgrade.current.background.Count > 0)
                {
                    objDes.SetActive(false);
                    objClaimed.SetActive(true);
                    return;
                }

                objDes.SetActive(true);
                objClaimed.SetActive(false);
                
                var apiEvent = FactoryApi.Get<ApiEvent>().Data;
                var valueTotal = GameConsts.MAX_NUMBER_BG_EVENT_YUKI;
                int valueReceived = apiEvent.event_data.background_yuki_claimed;
                int valueRemain = Mathf.Clamp(valueTotal - valueReceived, 0, valueTotal);
                bool isCanReceive = valueReceived < valueTotal;
                
          
                string remainValue = $"{Localization.Get(TextId.Event_RemainGift)} {valueRemain.ToFormat()}";
                if (isCanReceive)
                {
                    txtDes.text = Localization.Get(TextId.Event_YukiDesGuide);
                }
                else
                {
                    txtDes.text = Localization.Get(TextId.Event_YukiSoldOut);
                }
                
                string remainText=string.Format("<color={1}>{0}</color>", remainValue, isCanReceive ? "#5AFF0E" : "#FF7669");
                txtStatus.text = $"{Localization.Get(TextId.Event_ReceiveGift)} {valueReceived.ToFormat()}/{remainText}";
                
                ProcessButtonClaim();
            }
        }

        private void ProcessButtonClaim()
        {
            var apiGame = FactoryApi.Get<ApiGame>().Data.Info;
            bool canClaim = apiGame.CurrentCharRank > TypeLeagueCharacter.Char_1;
            objCanClaim.SetActive(canClaim);
            imgBtnClaim.material = canClaim ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
            btnClaim.interactable = canClaim;
        }

        
        private async void OnClaimBackground()
        {
            this.ShowProcessing();
            try
            {
                var apiEvent = FactoryApi.Get<ApiEvent>();
                await apiEvent.ClaimBackground();
                await FactoryApi.Get<ApiUpgrade>().Get();
                await apiEvent.Get();
                ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Event_YukiClaimSuccess));
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
    }
}