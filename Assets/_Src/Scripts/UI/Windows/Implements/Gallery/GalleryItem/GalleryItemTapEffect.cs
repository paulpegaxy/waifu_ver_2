using System;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class GalleryItemTapEffect : AGalleryItem<DataItemGalleryTapEffect>
    {
        [SerializeField] private GameObject objSelected;
        
        private DataItemGalleryTapEffect _data;
        
        public static Action OnChangeTapEffect;
        
        protected override void OnSetData(DataItemGalleryTapEffect data)
        {
            _data = data;
            txtName.text = data.config.name;
            
            // var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            // var userInfo = storageUserInfo.Get();
            // if (string.IsNullOrEmpty(userInfo.selectedBackgroundId))
            // {
            //     userInfo.selectedBackgroundId = _data.config.backgroundId;
            //     storageUserInfo.Save();
            // }
            //
            // bool isSelected = userInfo.selectedBackgroundId.Equals(bgId);
            
            objSelected.SetActive(data.isSelected);
            btnClick.interactable = !data.isSelected;
            
            if (!data.isSelected)
            {
                objLocked.SetActive(!_data.isUnlock);
            }
        }
        
        // private void ProcessSelect(UIPopup popup)
        // {
        //     var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
        //     var userInfo = storageUserInfo.Get();
        //     userInfo.selectedTapEffectId = _data.config.id;
        //
        //     storageUserInfo.Save();
        //     this.PostEvent(TypeGameEvent.ChangeTapEffect, true);
        //     OnChangeTapEffect?.Invoke();
        //     popup.Hide();
        //     ControllerPopup.ShowToastSuccess("Change effect successfully");
        // }
        
        protected override void OnClick()
        {
            if (!_data.isUnlock)
            {
                ControllerPopup.ShowToastError("This effect is locked");
                return;
            }

            ControllerPopup.ShowConfirm("Do you want to select this effect?", Localization.Get(TextId.Common_Select),
                onOk: (popup) =>
                {
                    SpecialExtensionGame.ProcessSelectTapEffect(_data.config.id, () =>
                    {
                        this.PostEvent(TypeGameEvent.ChangeTapEffect, true);
                        OnChangeTapEffect?.Invoke();
                        popup.Hide();
                    });
                });
        }
    }
    
    [Serializable]
    public class DataItemGalleryTapEffect : DataItemGallery
    {
        public DataItemTapEffect config;
        public bool isSelected;
        
        public DataItemGalleryTapEffect()
        {
            
        }
    }
}