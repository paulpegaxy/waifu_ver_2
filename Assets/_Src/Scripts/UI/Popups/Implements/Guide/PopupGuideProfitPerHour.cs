
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PopupGuideProfitPerHour : MonoBehaviour
    {
        [SerializeField] private UIButton btnGoto;
        [SerializeField] private TMP_Text[] arrTxtGuide;

        private void OnEnable()
        {
            btnGoto.onClickEvent.AddListener(OnClickGoto);
            LoadData();
        }

        private void OnDisable()
        {
            btnGoto.onClickEvent.RemoveListener(OnClickGoto);
        }

        private void LoadData()
        {
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            int profitPerHour = Mathf.RoundToInt(gameInfo.point_per_second * 3600);
            string sugarPerHourLabel = $" {Localization.Get(TextId.Common_ScName)}/h";
            var desProfitPremium = FactoryApi.Get<ApiUpgrade>().Data.GetTotalProfitFromPremiumChars().ToLetter();


            var profitForCard = profitPerHour - Mathf.FloorToInt(float.Parse(desProfitPremium));
            // string desProfitPerCard = profitForCard.ToFormat();
            string desProfitGirlBonus = "";
            
            if (gameInfo.current_girl_bonus > 0)
            {
                var bonusFromGirl = profitPerHour * ((float) gameInfo.current_girl_bonus / 100);
                desProfitGirlBonus = Mathf.FloorToInt(bonusFromGirl).ToFormat();
                desProfitGirlBonus += sugarPerHourLabel;
                desProfitGirlBonus += $" ({gameInfo.current_girl_bonus}%)";
            }
            else
            {
                desProfitGirlBonus = $"0 {sugarPerHourLabel} (0%)";
            }


            arrTxtGuide[0].text = SetHighLightString(Localization.Get(TextId.Idleearn_GuideUpgrade), 
                profitForCard.ToFormat() + sugarPerHourLabel);

            arrTxtGuide[1].text = SetHighLightString(Localization.Get(TextId.Idleearn_GuidePremiumWaifu),
                desProfitPremium + sugarPerHourLabel);

            arrTxtGuide[2].text = SetHighLightString(Localization.Get(TextId.Idleearn_GuideSugarPerH),
                gameInfo.ProfitPerHourParse.ToFormat() + sugarPerHourLabel);
            
            arrTxtGuide[3].text = SetHighLightString(Localization.Get(TextId.Idleearn_GuideWaifuBonus), 
                desProfitGirlBonus);
            
            arrTxtGuide[4].text = SetHighLightString(Localization.Get(TextId.Idleearn_GuidePassiveIncome), 
                "3 Hours/day");
        }

        private void OnClickGoto()
        {
            GetComponent<UIPopup>().Hide();
            Signal.Send(StreamId.UI.OpenUpgrade);
        }


        private string SetHighLightString(string lb, string des)
        {
            return $"{lb.SetHighlightStringPink()}: {des.SetHighlightStringGreen3_3FA509()}";
        }
    }
}
