using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GalleryItemWaifuPremium : AGalleryItem<DataItemGalleryWaifuPremium>
    {
        [SerializeField] private GameObject objSelected;
        [SerializeField] private Image imgHolderName;
        [SerializeField] private GameObject objPromoteName;
        
        private DataItemGalleryWaifuPremium _data;

        protected override void OnSetData(DataItemGalleryWaifuPremium data)
        {
            _data = data;

            objCompleted.SetActive(false);
            objLocked.SetActive(false);

            LoadStatusSelect(data.girlId);

            bool isEnded = false;
            bool isUnlocked = data.data != null;
            if (isUnlocked)
            {
                txtName.text = data.data.GetCharPremium().name;
            }
            else
            {
               
#if UNITY_EDITOR && !PRODUCTION_BUILD
                var itemFind = FactoryApi.Get<ApiShop>().Data.GetItemByItemType(TypeShopItem.PremiumFierenTest);
                isEnded = itemFind?.IsSoldOut ?? true;
#else
                var itemFind = FactoryApi.Get<ApiShop>().Data.GetItemByItemType(TypeShopItem.PremiumFieren);
                isEnded = itemFind?.IsSoldOut ?? true;
#endif
                if (isEnded)
                    txtName.text = Localization.Get(TextId.Gallery_EventEnded);
                else
                    txtName.text = Localization.Get(TextId.Gallery_LbNewChar);
                imgHolderName.material = isEnded ? DBM.Config.visualConfig.materialConfig.matDisableObject : null;
            }
            
            objPromoteName.SetActive(!isUnlocked);

            btnClick.interactable = !isEnded;
        }

        private void LoadStatusSelect(int girlId)
        {
            var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
            bool isSelected = userInfo.selectedWaifuId == girlId;
            
            objSelected.SetActive(isSelected);
        }

        protected override void OnClick()
        {
            if (_data.data == null)
            {
                SpecialExtensionShop.ShowPopupOfferCharFreerin().Forget();
                return;
            }
            
            this.PostEvent(TypeGameEvent.GalleryDetail, _data);
            Signal.Send(StreamId.UI.GalleryDetail);
        }
    }
    
    [Serializable]
    public class DataItemGalleryWaifuPremium : DataItemGallery
    {
        public ModelApiUpgradePremiumChar data;

        public DataItemGalleryWaifuPremium()
        {

        }
    }
}