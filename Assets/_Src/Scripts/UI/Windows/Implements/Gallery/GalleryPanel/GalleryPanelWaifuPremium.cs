using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GalleryPanelWaifuPremium : AGalleryPanel
    {
       
        
        protected override async UniTask OnLoadData()
        {
            ListData.Add(new ModelGalleryCellViewHeader()
            {
                FilterType = TypeFilterGallery.WaifuPremium
            });

            var list = ProcessListItem();
            var groupedItems = new List<List<DataItemGalleryWaifuPremium>>();
            for (var i = 0; i < list.Count; i += 3)
            {
                var sublist = list.Skip(i).Take(3).ToList();
                groupedItems.Add(sublist);
            }
          
            foreach (var group in groupedItems)
            {
                ListData.Add(new ModelGalleryCellViewCotentWaifuPremium()
                {
                    RowItemData = group
                });
            }
            
            scroller.SetData(ListData);
        }

        private List<DataItemGalleryWaifuPremium> ProcessListItem()
        {
            var listConfig = DBM.Config.charPremiumConfig.ListCharConfig;
            var list = new List<DataItemGalleryWaifuPremium>();
            for (var i = 0; i < listConfig.Count; i++)
            {
                var ele = listConfig[i];
                list.Add(new DataItemGalleryWaifuPremium()
                {
                    girlId = ele.charId,
                    name = ele.name,
                    avatar_name = $"char_{ele.charId}",
                    isUnlock = true,
                });
            }

            var upgradeInfo = FactoryApi.Get<ApiUpgrade>().Data.premium_waifu;
            int count = Mathf.Min(upgradeInfo.Count, list.Count);

            for (int i = 0; i < count; i++)
            {
                var ele = upgradeInfo[i];
                list[i].data = ele;
            }

            return list;
        }
    }
}