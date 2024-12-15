// Author: ad   -
// Created: 16/09/2024  : : 23:09
// DateUpdate: 16/09/2024

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class PartnerMergePalPanelQuest : APartnerMergePalPanel
    {
        protected override async UniTask OnProcessLoadPartner(MainWindowAction type)
        {
            ProcessHeaderContent();
            mergePalScroller.SetData(ListData);
            this.ShowProcessing();
            try
            {
                if (!IsWaitingLoadData)
                {
                    IsWaitingLoadData = true;
                    await ProcessQuestContent();
                }

                mergePalScroller.SetData(ListData);
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        private void ProcessHeaderContent()
        {
            if (ParterData.empty_filter)
            {
                ListData.Add(new ModelPartnerMergePalHeaderEmptyFilter()
                {
                    eventConfig = ParterData
                });
            }
            else
            {
                ListData.Add(new ModelPartnerMergePalCellViewHeaderCollab()
                {
                    FilterType = TypeFilterPartner.Quest,
                    eventConfig = ParterData,
                    IsHaveRankingFilter = ParterData.IsHaveRanking
                });
            }
        }

        private async UniTask ProcessQuestContent()
        {
            
            var apiQuest = FactoryApi.Get<ApiQuest>();
            await apiQuest.Get();
            await apiQuest.EmojiCheck();
            var questData = apiQuest.Data.Quest;
            if (questData == null)
                return;

            var listQuestData = SpecialExtensionGame.GetQuestPartnerEventList(questData, ParterData);
            
            // bool isHavePrivate = FactoryApi.Get<ApiUser>().Data.User.IsHavePrivatePartner(ParterData.GetPartnerTagPrivate);
            // if (isHavePrivate)
            // {
            //     var listPrivate = FactoryApi.Get<ApiQuest>().Data.GetListQuestEvent(ParterData.PrivatePartnerId);
            //     listQuestData.AddRange(listPrivate);
            // }
            
            for (int i = 0; i < listQuestData.Count; i++)
            {
                ListData.Add(new ModelPartnerMergePalCellViewContentQuest()
                {
                    QuestData = listQuestData[i],
                });
            }
            IsWaitingLoadData = false;
        }
    }
}