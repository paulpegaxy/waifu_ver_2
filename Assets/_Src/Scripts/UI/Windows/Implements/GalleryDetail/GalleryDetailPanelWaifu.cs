// Author: ad   -
// Created: 16/10/2024  : : 22:10
// DateUpdate: 16/10/2024

using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Runtime;
using Sirenix.OdinInspector;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GalleryDetailPanelWaifu : AGalleryDetailPanel
    {
        // [SerializeField] private GameObject objRank;
        // [SerializeField] private Image imgRank;
        // [SerializeField] private TMP_Text txtRank;
        
        protected override async UniTask OnFetchData()
        {
            if (Data is DataItemGalleryWaifu data)
            {
                LoadData(data);
            }
        }
        
        private void LoadData(DataItemGalleryWaifu data)
        {
            LoadStatusData(data.isDone,data.girlId);

            SpawnGirl(() =>
            {
                // imgRank.LoadSpriteAutoParseAsync($"league_{(int)data.charRankType}");
                // txtRank.color = data.colorRank;
                // objRank.SetActive(true);
                
                LevelSelected = 0;
                var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
                if (data.isDone)
                {
                    MaxLevelGirl = GameConsts.MAX_LEVEL_PER_CHAR - 3;
                    // MaxLevelGirl = GameConsts.MAX_LEVEL_PER_CHAR - 1;
                }
                else
                {
                    // MaxLevelGirl = Math.Clamp(gameInfo.current_level_girl % GameConsts.MAX_LEVEL_PER_CHAR, 0, GameConsts.MAX_LEVEL_PER_CHAR);
                    MaxLevelGirl = Math.Clamp(gameInfo.current_level_girl % GameConsts.MAX_LEVEL_PER_CHAR, 0, GameConsts.MAX_LEVEL_PER_CHAR-2);
                }

                // UnityEngine.Debug.LogError("IS Done : "+data.isDone+ ", Is Max Level" + _maxLevelGirl + ", currLv: " + gameInfo.current_level_girl);

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
        
        private void LoadStatusData(bool isCompleted,int girlId)
        {
            if (isCompleted)
            {
                btnSelect.gameObject.SetActive(false);
                objSelected.SetActive(true);
                txtBtnSelected.text = Localization.Get(TextId.Gallery_LevelCompleted);
            }
            else
            {
                var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
                bool isSelected = (userInfo.selectedWaifuId == girlId || userInfo.selectedWaifuId == 0) &&
                                  !userInfo.isChoosePremiumWaifu;
                
                btnSelect.gameObject.SetActive(!isSelected);
                objSelected.SetActive(isSelected);
                
                txtBtnSelected.text = Localization.Get(TextId.Gallery_Selected);
            }
        }

        protected override void Clear(bool isClear)
        {
            // objRank.SetActive(!isClear);
            holderBtn.SetActive(!isClear);
        }

        protected override void OnSelectCharacter()
        {
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            userInfo.selectedWaifuId = Data.girlId;
            userInfo.isChoosePremiumWaifu = false;
        
            storageUserInfo.Save();
            
            var charName = DBM.Config.rankingConfig.GetRankDataBasedGirlId(Data.girlId);
            ControllerPopup.ShowToastSuccess(string.Format(Localization.Get(TextId.Gallery_NotiSelected), charName.girlName));
        }
    }
}