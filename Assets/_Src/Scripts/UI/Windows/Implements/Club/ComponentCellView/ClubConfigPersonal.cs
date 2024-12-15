using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Game.Model;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClubConfigPersonal : MonoBehaviour
{
    [SerializeField] private ItemAvatar itemAvatar;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private Image sliderProgress;
    [SerializeField] private TMP_Text txtRankName;
    [SerializeField] private TMP_Text txtCharName;
    [SerializeField] private GameObject objLock;

    public async void SetData(ModelApiLeaderboardData data,TypeLeagueCharacter girlType)
    {
        txtScore.text = data.PointsParse.ToLetter();

        var apiGame = FactoryApi.Get<ApiGame>();
        // await apiGame.GetInfo();
        var gameInfo = apiGame.Data.Info;
        if (gameInfo != null)
        {
            var totalPointComplete = BigDouble.Parse(gameInfo.DataCurrentGirlSelected.toValue.ToString());
            
            // var dataVisual = DBM.Config.rankingConfig.GetRankData(gameInfo.CurrentGirlId);
            var dataVisual = DBM.Config.rankingConfig.GetRankData(girlType);
            
            txtCharName.text = dataVisual.girlName;
            txtRankName.text = "RANK " + (int)girlType;
            itemAvatar.SetImageAvatar(dataVisual.girlId);
            // txtRankName.text = "RANK " + ((gameInfo.current_level_girl / 10) + 1);

            // var pointNeed = DBM.Config.rankingConfig.GetRankData(girlType);
            
            // txtScore.text = $"{gameInfo.PointAllTimeParse.ToLetter()}/{totalPointComplete.ToLetter()}";
            //
            // var valueTotal = float.Parse(gameInfo.point_all_time);
            // var valueCurrent = float.Parse(gameInfo.point);

            // sliderProgress.fillAmount = Mathf.Clamp01(valueCurrent / valueTotal);

            // int modValue = (gameInfo.current_level_girl + 1) % 10;
            // if (modValue == 0)
            // {
            //     txtScore.text = "10/10";
            //     sliderProgress.fillAmount = 1;
            // }
            // else
            // {
            //     txtScore.text = $"Lv: {modValue}/{GameConsts.MAX_LEVEL_PER_CHAR}";
            //     sliderProgress.fillAmount = Mathf.Clamp01((float) modValue / GameConsts.MAX_LEVEL_PER_CHAR);
            // }

            ProcessPointScore(girlType, gameInfo);

            bool isUnlock = ((int)girlType) <= (int)gameInfo.CurrentCharRank;
            objLock.SetActive(!isUnlock);
        }
        
    }

    private void ProcessPointScore(TypeLeagueCharacter girlType,   ModelApiGameInfo gameInfo)
    {
        var pointNeed = DBM.Config.rankingConfig.GetRankData(girlType);
        BigDouble totalRemain = 0;
        int index = gameInfo.current_level_girl / GameConsts.MAX_LEVEL_PER_CHAR;
        for (int i = index; i < pointNeed.listPointVisualLevel.Count; i++)
        {
            totalRemain += pointNeed.listPointVisualLevel[i];
        }

        txtScore.text = $"{gameInfo.PointParse.ToLetter()}/{totalRemain.ToLetter()}";


        sliderProgress.fillAmount = Mathf.Clamp01(float.Parse(gameInfo.point) / float.Parse(totalRemain.ToString()));
    }
}
