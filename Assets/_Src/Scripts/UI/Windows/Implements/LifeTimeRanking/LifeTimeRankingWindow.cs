using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Game.UI.Windows.Implements.LifeTimeRanking;
using TMPro;
using UnityEngine;

public class LifeTimeRankingWindow : UIWindow
{
    [SerializeField] private LifetimeRankingScroller scroller;
    [SerializeField] private LifetimeRankingCellView myRanking;
    [SerializeField] private UIButton btnShare;
    [SerializeField] private UIButton btnInfo;

    private int _myRankIndex;
    
    protected override async void OnEnabled()
    {
        this.ShowProcessing();
        try
        {
            var apiLeaderboard = FactoryApi.Get<ApiLeaderboard>();
            var leaderboard = await apiLeaderboard.GetIndividualAllTime();

            var data = new List<ModelLeaderboardAllTime>();
            foreach (var item in leaderboard.leaderboard)
            {
                data.Add(new ModelLeaderboardAllTime()
                {
                    Rank = item.rankPos,
                    Name = item.user.username,
                    LifeTimeScore = item.PointsParse,
                });
            }
            
            myRanking.gameObject.SetActive(true);
            btnShare.gameObject.SetActive(true);
            
            if (leaderboard.global_rank != null)
            {
                _myRankIndex = (int)leaderboard.global_rank;        
           
                myRanking.SetData(new ModelLeaderboardAllTime()
                {
                    Rank = _myRankIndex,
                    Name = leaderboard.current_user_leaderboard.user.username,
                    LifeTimeScore = leaderboard.current_user_leaderboard.PointsParse
                });
            }
            else
            {
                _myRankIndex = 9999;
                myRanking.SetData(new ModelLeaderboardAllTime()
                {
                    Name = leaderboard.current_user_leaderboard.user.username,
                    LifeTimeScore = leaderboard.current_user_leaderboard.PointsParse,
                    Rank = _myRankIndex
                });
            }
			
            scroller.SetData(data);
            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
        btnShare.onClickEvent.AddListener(OnShare);
        btnInfo.onClickEvent.AddListener(OnInfo);
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        btnShare.onClickEvent.RemoveListener(OnShare);
        btnInfo.onClickEvent.RemoveListener(OnInfo);
    }
    
    private void OnShare()
    {
        // Debug.LogError("OnShare: " + TelegramWebApp.IsMobile());
        if (TelegramWebApp.IsMobile())
        {
            // var text =
            //     $"I’m {_myRankIndex} rank on the Pocket Waifu leaderboard! Join me on the #1 Waifu game on Telegram and earn with me!" +
            //     $"\n{0}";
            // var mediaUrl = $"https://cdn.mirailabs.co/waifu-tap/static/pocket%20waifu%20story.mp4";

            var status = SpecialExtensionGame.ShareToStoryForRanking(_myRankIndex);
            if (!status)
            {
                ControllerPopup.ShowToastError("Can not share story at this time");
            }
            
        }
        else
        {
            ControllerPopup.ShowToastError("Only use on mobile");
        }
    }

    private void OnInfo()
    {
        ControllerPopup.ShowInformation(Localization.Get(TextId.Toast_GuideRanking));
    }
}
