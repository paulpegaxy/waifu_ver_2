// Author: ad   -
// Created: 16/10/2024  : : 22:10
// DateUpdate: 16/10/2024

using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class GalleryDetailPanelWaifuPremium : AGalleryDetailPanel
    {
        private string _charName;
        
        protected override async UniTask OnFetchData()
        {   
            if (Data is DataItemGalleryWaifuPremium data)
            {
                LoadData(data);
            }
        }
        
        private void LoadData(DataItemGalleryWaifuPremium data)
        {
            _charName = data.data.GetCharPremium().name;
            LoadStatusSelect(data.girlId);
            SpawnGirl(() =>
            {
                LevelSelected = 0;
                var upgradeInfo = data.data;
                bool isDoneGirl = upgradeInfo.level == GameConsts.MAX_LEVEL_PER_CHAR;

                if (isDoneGirl)
                {
                    // MaxLevelGirl = GameConsts.MAX_LEVEL_PER_CHAR - 1;
                    MaxLevelGirl = GameConsts.MAX_LEVEL_PER_CHAR - 3;
                }
                else
                {
                    // MaxLevelGirl = Math.Clamp((upgradeInfo.level - 1) % GameConsts.MAX_LEVEL_PER_CHAR, 0,
                    //     GameConsts.MAX_LEVEL_PER_CHAR);
                    MaxLevelGirl = Math.Clamp((upgradeInfo.level - 1) % GameConsts.MAX_LEVEL_PER_CHAR, 0,
                        GameConsts.MAX_LEVEL_PER_CHAR-2);
                }

                if (MaxLevelGirl > 0)
                {
                    holderBtn.SetActive(true);
                    posContainIndicator.gameObject.SetActive(true);
                    ProcessIndicator();
                    listIndicatorActive[0].SetSelected(true);
                }
                else
                    posContainIndicator.gameObject.SetActive(false);
            });
        }

        private void LoadStatusSelect(int girlId)
        {
            var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
            bool isSelected = userInfo.selectedWaifuId == girlId;
            btnSelect.gameObject.SetActive(!isSelected);
            objSelected.SetActive(isSelected);
            txtBtnSelected.text = Localization.Get(TextId.Gallery_Selected);
        }

        protected override void Clear(bool isClear)
        {
            holderBtn.SetActive(!isClear);
        }

        protected override void OnSelectCharacter()
        {
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            userInfo.selectedWaifuId = Data.girlId;
            userInfo.isChoosePremiumWaifu = true;

            storageUserInfo.Save();
            
            ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Gallery_NotiSelected), _charName));
        }
    }
}