// Author: ad   -
// Created: 28/09/2024  : : 16:09
// DateUpdate: 28/09/2024

using System;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GalleryItemLocation : AGalleryItem<DataItemGalleryLocation>
    {
        [SerializeField] private GameObject objSelected;
        [SerializeField] private RawImage imgBg;
        
        private DataItemGalleryLocation _data;
        
        public static Action OnChangeBackground;

        protected override void OnSetData(DataItemGalleryLocation data)
        {
            _data = data;
            txtName.text = data.config.name;

            // imgBg.DOFade(0, 0);
            
            LoadInfo(data.config.backgroundId);
            
            if (data.isDefaultBg)
            {
                string keyBg = FactoryApi.Get<ApiGame>().Data.Info.CurrentGirlId + "_bg";
                // string keyBg = "20001_bg";
                ProcessBackground(keyBg);
            }
            else
            {
                if (data.config.IsYukiBackground())
                    ProcessBackground(data.config.backgroundId);
                else
                    ProcessBackground(data.config.backgroundId + "_bg");
            }
        }

        private async void ProcessBackground(string bgKey)
        {
            imgBg.texture = await ControllerSpawner.Instance.SpawnBgSpecialGirl(bgKey);
            // imgBg.DOFade(1, 0.1f);
        }
        
        private void LoadInfo(string bgId)
        {
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            if (string.IsNullOrEmpty(userInfo.selectedBackgroundId))
            {
                userInfo.selectedBackgroundId = _data.config.backgroundId;
                storageUserInfo.Save();
            }
            
            bool isSelected = userInfo.selectedBackgroundId.Equals(bgId);
            
            objSelected.SetActive(isSelected);
            btnClick.interactable = !isSelected;
            
            if (!isSelected)
            {
                objLocked.SetActive(!_data.isUnlock);
            }
        }

        private void ProcessSelectBackground(UIPopup popup)
        {
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            userInfo.selectedBackgroundId = _data.config.backgroundId;
            userInfo.isChooseSpecialBg = !_data.isDefaultBg;

            storageUserInfo.Save();
            this.PostEvent(TypeGameEvent.ChangeBackground, true);
            OnChangeBackground?.Invoke();
            popup.Hide();
            ControllerPopup.ShowToastSuccess("Change background successfully");
        }

        protected override void OnClick()
        {
            if (string.IsNullOrEmpty(_data.myBgId))
            {
                ControllerPopup.ShowToastError("Background is locked");
                return;
            }

            ControllerPopup.ShowConfirm("Do you want to select this background?",
                Localization.Get(TextId.Common_Select),
                onOk: ProcessSelectBackground);
        }
    }

    [Serializable]
    public class DataItemGalleryLocation : DataItemGallery
    {
        public DataItemBackground config;
        public bool isDefaultBg;
        public string myBgId;
        
        public DataItemGalleryLocation()
        {
            
        }
    }
}