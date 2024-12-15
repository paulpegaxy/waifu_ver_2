using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class GalleryPanelLocation : AGalleryPanel
    {
        protected override async UniTask OnLoadData()
        {
            ListData.Add(new ModelGalleryCellViewHeader()
            {
                FilterType = TypeFilterGallery.Location
            });

            
            var dataListItem = new List<DataItemGalleryLocation>();
            var listBgConfig = DBM.Config.backgroundConfig.ListBgConfig;
            var userData = FactoryApi.Get<ApiUpgrade>().Data.current;

            var bgDefaultConfig = listBgConfig[0];
            dataListItem.Add(new DataItemGalleryLocation()
            {
                isUnlock = true,
                isDefaultBg = true,
                config = bgDefaultConfig,
                myBgId = bgDefaultConfig.backgroundId
            });
            
            for (var i = 1; i < listBgConfig.Count; i++)
            {
                var ele = listBgConfig[i];
                var isUnlock = userData.IsHaveBackground(ele.backgroundId);
                string myBgId = isUnlock ? ele.backgroundId : string.Empty;
                var data = new DataItemGalleryLocation
                {
                    config = ele,
                    isUnlock = isUnlock,
                    myBgId = myBgId,
                    isDefaultBg = false
                };
                dataListItem.Add(data);
            }

            if (dataListItem.Count > 0)
            {
                var groupedItems = new List<List<DataItemGalleryLocation>>();
                for (var i = 0; i < dataListItem.Count; i += 3)
                {
                    var sublist = dataListItem.Skip(i).Take(3).ToList();
                    groupedItems.Add(sublist);
                }

                foreach (var group in groupedItems)
                {
                    ListData.Add(new ModelGalleryCellViewContentLocation()
                    {
                        RowItemData = group
                    });
                }
            }
            else
            {
                ListData.Add(new ModelGalleryCellViewContentEmpty());
            }
            
            scroller.SetData(ListData);
        }
    }
}