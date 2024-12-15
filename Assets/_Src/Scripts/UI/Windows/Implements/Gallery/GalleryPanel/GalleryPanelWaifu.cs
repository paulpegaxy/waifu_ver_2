using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class GalleryPanelWaifu : AGalleryPanel
    {
        protected override async UniTask OnLoadData()
        {
            ListData.Add(new ModelGalleryCellViewHeader()
            {
                FilterType = TypeFilterGallery.Waifu
            });

            ProcessListItem();
            
            // ProcessListComingSoon();
            
            scroller.SetData(ListData);
        }

        private void ProcessListItem()
        {
            var apiGame = FactoryApi.Get<ApiGame>().Data.Info;
            var listVisualConfig = DBM.Config.rankingConfig.GetListExistGirl();
            var gameInfo = apiGame;

            var index = ((int)gameInfo.CurrentCharRank) - 1;
            var dataListItem = new List<DataItemGalleryWaifu>();
            for (var i = 0; i < listVisualConfig.Count; i++)
            {
                var visualConfig = listVisualConfig[i];
                var data = new DataItemGalleryWaifu
                {
                    avatar_name = $"char_{visualConfig.girlId}",
                    // charRankType = visualConfig.type,
                    girlId = visualConfig.girlId,
                    name = visualConfig.girlName,
                    isUnlock = i <= index,
                    // colorRank = visualConfig.rankColor,
                    isDone = i < index,
                };
                dataListItem.Add(data);
            }
            
            //coming soon
            var listConfig = DBM.Config.charPremiumConfig.ListCharComingSoonConfig;
            for (var i = 0; i < listConfig.Count; i++)
            {
                var visualConfig = listConfig[i];
                var data = new DataItemGalleryWaifu
                {
                    avatar_name = $"char_{visualConfig.charId}",
                    girlId = visualConfig.charId,
                    name = visualConfig.name,
                    isUnlock = false,
                    isDone = false,
                    isComingSoon = true
                };
                dataListItem.Add(data);
            }
            
            var groupedItems = new List<List<DataItemGalleryWaifu>>();
            for (var i = 0; i < dataListItem.Count; i += 3)
            {
                var sublist = dataListItem.Skip(i).Take(3).ToList();
                groupedItems.Add(sublist);
            }

            foreach (var group in groupedItems)
            {
                ListData.Add(new ModelGalleryCellViewContentWaifu()
                {
                    RowItemData = group
                });
            }
        }

        private void ProcessListComingSoon()
        {
            // var listVisualConfig = DBM.Config.rankingConfig.GetListExistGirl();
            var dataListItem = new List<DataItemGalleryWaifu>();
            var listConfig = DBM.Config.charPremiumConfig.ListCharComingSoonConfig;
            for (var i = 0; i < listConfig.Count; i++)
            {
                var visualConfig = listConfig[i];
                var data = new DataItemGalleryWaifu
                {
                    avatar_name = $"char_{visualConfig.charId}",
                    girlId = visualConfig.charId,
                    name = visualConfig.name,
                    isUnlock = false,
                    isDone = false,
                    isComingSoon = true
                };
                dataListItem.Add(data);
            }

            var groupedItems = new List<List<DataItemGalleryWaifu>>();
            for (var i = 0; i < dataListItem.Count; i += 3)
            {
                var sublist = dataListItem.Skip(i).Take(3).ToList();
                groupedItems.Add(sublist);
            }
            
            foreach (var group in groupedItems)
            {
                ListData.Add(new ModelGalleryCellViewContentWaifu()
                {
                    RowItemData = group
                });
            }
        }
    }
}