using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;
using Game.UI.Windows.Implements.LifeTimeRanking;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalPanelRanking : APartnerMergePalPanel
    {
        [SerializeField] private LifetimeRankingCellView myRanking;

        protected override async UniTask OnProcessLoadPartner(MainWindowAction type)
        {
            ListData.Add(new ModelPartnerMergePalCellViewHeaderRanking()
            {
                FilterType = TypeFilterPartner.Ranking,
                eventConfig =ParterData
            });
            mergePalScroller.SetData(ListData);
            try
            {
                this.ShowProcessing();
                if (!IsWaitingLoadData)
                {
                    IsWaitingLoadData = true;
                    await ProcessContent();
                }

                mergePalScroller.SetData(ListData);
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        private async UniTask ProcessContent()
        {
            var apiEvent = FactoryApi.Get<ApiEvent>();
            var leaderboardData =await apiEvent.GetEventLeaderboard(ParterData.id);
            for (int i = 0; i < leaderboardData.Count; i++)
            {
                var item = leaderboardData[i];
                ListData.Add(new ModelPartnerMergePalCellViewContentRanking()
                {
                    Data = new ModelLeaderboardAllTime()
                    {
                        Rank = item.rank,
                        Name = item.user.username,
                        LifeTimeScore = item.ValueParse,
                    }
                });
            }
            
            var dataMyRank = leaderboardData.Find(
                x => x.user.id == SpecialExtensionGame.MyUserId);
            myRanking.gameObject.SetActive(dataMyRank != null);
            if (dataMyRank != null)
            {
                myRanking.SetData(new ModelLeaderboardAllTime()
                {
                    Rank = dataMyRank.rank,
                    Name = dataMyRank.user.username,
                    LifeTimeScore = dataMyRank.ValueParse,
                });
            }
            
            IsWaitingLoadData = false;
        }
    }
}