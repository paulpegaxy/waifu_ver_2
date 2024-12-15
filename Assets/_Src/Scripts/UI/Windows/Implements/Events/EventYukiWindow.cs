using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class EventYukiWindow : UIWindow
    {
        [SerializeField] private EventYukiScroller scroller;
        
        protected List<AModelEventYukiCellView> ListData;
        
        private bool _isWaitingLoadData;

        protected override void OnEnabled()
        {
            Refresh();
            ModelApiEvent.OnChanged += OnRefreshData;
            QuestExecute.OnRefresh += Refresh;
        }

        protected override void OnDisabled()
        {
            ModelApiEvent.OnChanged -= OnRefreshData;
            QuestExecute.OnRefresh -= Refresh;
        }
        
        private void OnRefreshData(ModelApiEvent data)
        {
            Refresh();
        }

        private async void Refresh()
        {
            var apiEvent = FactoryApi.Get<ApiEvent>().Data;
            ListData = new List<AModelEventYukiCellView>();
            ListData.Add(new ModelEventYukiCellViewHeader()
            {
                eventConfig = apiEvent.EventMeetYuki
            });

            if (!_isWaitingLoadData)
            {
                _isWaitingLoadData = true;
                await ProcessQuestContent();
            }
  

            ListData.Add(new ModelEventYukiCellViewContentYukiBackground());
            
            // ProcessShopBundle();
            
            scroller.SetData(ListData);
        }
        
        private async UniTask ProcessQuestContent()
        {
            string eventId = FactoryApi.Get<ApiEvent>().Data.EventMeetYuki.id;
            var apiQuest = FactoryApi.Get<ApiQuest>();
            await apiQuest.Get();
            var questData = apiQuest.Data.Quest;
            if (questData == null)
                return;
            
            var listQuestData = SpecialExtensionGame.GetQuestEventList(questData, eventId);
            for (int i = 0; i < listQuestData.Count; i++)
            {
                ListData.Add(new ModelEventYukiCellViewContentQuest()
                {
                    QuestData = listQuestData[i],
                });
            }
            _isWaitingLoadData = false;
        }
        
        private void ProcessShopBundle()
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            if (apiShop.Data == null)
                return;

            var apiEvent = FactoryApi.Get<ApiEvent>().Data;
            var shopList = apiShop.Data.GetListShopItemBasedEvent(apiEvent.EventMeetYuki.id);
            
            var itemBotTap = shopList.FirstOrDefault(x => x.GetPackType() == TypeShopPack.TapBotPrimePack);
            if (itemBotTap != null)
            {
                ListData.Add(new ModelEventYukiCellViewContentBundleBotTap()
                {
                    Data = itemBotTap
                });
            }

            var groupItemTimelapse = shopList.Where(x => x.GetPackType() == TypeShopPack.TimeLapse).ToList();
            if (groupItemTimelapse.Count > 0)
            {
                ListData.Add(new ModelEventYukiCellViewContentBundleTimelapse()
                {
                    ListItemData = groupItemTimelapse
                });
            }
        }
    }

}