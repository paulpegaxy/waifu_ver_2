using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class GalleryPanelTapEffect : AGalleryPanel
    {
        protected override async UniTask OnLoadData()
        {
            ListData.Add(new ModelGalleryCellViewHeader()
            {
                FilterType = TypeFilterGallery.TapEffect
            });
            
            var dataListItem = new List<DataItemGalleryTapEffect>();
            var lisTapEffectConfig = DBM.Config.tapEffectConfig.ListTapEffConfig;
            var userData = FactoryApi.Get<ApiUpgrade>().Data.current;

            var selectedId = FactoryStorage.Get<StorageUserInfo>().Get().selectedTapEffectId;
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            if (userInfo.selectedTapEffectId==0)
            {
                selectedId = lisTapEffectConfig[0].id;
                userInfo.selectedTapEffectId = selectedId;
                storageUserInfo.Save();
            }
            
            for (var i = 0; i < lisTapEffectConfig.Count; i++)
            {
                var ele = lisTapEffectConfig[i];
                var isUnlock = userData.IsHaveTapEffect(ele.id.ToString());
                bool isSelected = selectedId == ele.id;
                
                var data = new DataItemGalleryTapEffect()
                {
                    config = ele,
                    isUnlock = isUnlock || ele.isDefault,
                    isSelected = isSelected,
                    avatar_name = ele.SlotTapAssetKey()
                };
                dataListItem.Add(data);
            }
            
            if (dataListItem.Count > 0)
            {
                var groupedItems = new List<List<DataItemGalleryTapEffect>>();
                for (var i = 0; i < dataListItem.Count; i += 3)
                {
                    var sublist = dataListItem.Skip(i).Take(3).ToList();
                    groupedItems.Add(sublist);
                }
            
                foreach (var group in groupedItems)
                {
                    ListData.Add(new ModelGalleryCellViewContentTapEffect()
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