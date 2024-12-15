using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClubConfig : MonoBehaviour
{
    [SerializeField] private TMP_Text txtRankName;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private Image imgRank;
    [SerializeField] private Image sliderProgress;

    public void SetData(ModelApiClubData clubData)
    {
        txtRankName.text = clubData.name;  //club name
        // imgRank.sprite = ControllerSprite.Instance.GetLeagueIcon(clubData.league);
        imgRank.LoadSpriteAutoParseAsync("league_" + clubData.league);
        FactoryApi.Get<ApiLeaderboard>().Data.Config
            .GetConfigClub(clubData.league, out ModelApiLeaderboardConfigData configData);
        if (configData != null)
        {
            var maxRange = float.Parse(configData.to_point);
            var current = float.Parse(clubData.total_point);
            txtScore.text = $"{clubData.TotalPointParse.ToLetter()}/{BigDouble.Parse(maxRange.ToString()).ToLetter()}";
            sliderProgress.fillAmount = Mathf.Clamp01(current / maxRange);
        }
    }
}
