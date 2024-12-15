using System;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GalleryItemWaifu : AGalleryItem<DataItemGalleryWaifu>
    {
        // [SerializeField] private Image imgRank;
        // [SerializeField] private TMP_Text txtRank;
        [SerializeField] private GameObject objSelected;

        private DataItemGalleryWaifu _data;

        // private void IsLockItem(bool isLock)
        // {
        //     imgRank.transform.parent.gameObject.SetActive(!isLock);
        // }

        protected override void OnSetData(DataItemGalleryWaifu data)
        {
            _data = data;
            
            // imgRank.LoadSpriteAutoParseAsync($"league_{(int)data.charRankType}");
            
            LoadStatusData();
            
            // txtRank.color = data.colorRank;
            
            btnClick.interactable = true;

            if (data.isComingSoon)
            {
                txtName.text = Localization.Get(TextId.Common_ComingSoon);
            }
            
            
            // IsLockItem(!data.isUnlock);
        }

        private void LoadStatusData()
        {
            if (!_data.isUnlock)
            {
                objCompleted.SetActive(false);
                objSelected.SetActive(false);
                return;
            }
            
            if (_data.isDone)
            {
                objCompleted.SetActive(true);
                objSelected.SetActive(false);
            }
            else
            {
                var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
                bool isSelected = (userInfo.selectedWaifuId == _data.girlId || userInfo.selectedWaifuId == 0) &&
                                  !userInfo.isChoosePremiumWaifu;
                
                objSelected.SetActive(isSelected);
                objCompleted.SetActive(false);
            }
        }

        protected override void OnClick()
        {
            if (_data.isComingSoon)
            {
                ControllerPopup.ShowToastComingSoon();
                return;
            }
            
            if (_data.isUnlock)
            {
                this.PostEvent(TypeGameEvent.GalleryDetail, _data);
                Signal.Send(StreamId.UI.GalleryDetail);
            }
            else
            {
                int prevIndex = (_data.girlId % 20000) - 1;
                string text = string.Format(Localization.Get(TextId.Gallery_NotiPleaseUnlock), prevIndex);
                ControllerPopup.ShowToastError(text);
            }
        }
    }

    [Serializable]
    public class DataItemGalleryWaifu : DataItemGallery
    {
        // public TypeLeagueCharacter charRankType;
        // public Color colorRank;
        public bool isDone;

        public bool isComingSoon;
        
        // public bool isCompletedChar;

        public DataItemGalleryWaifu()
        {

        }
    }
}


