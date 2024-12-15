// Author: ad   -
// Created: 14/12/2024  : : 17:12
// DateUpdate: 14/12/2024

using System;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UserProfileCellViewHeader : MonoBehaviour
    {
        [SerializeField] private Image imgAva;
        [SerializeField] private UIButton btnEditAva;
        [SerializeField] private UIButton btnEditName;
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtLv;
        [SerializeField] private TMP_Text txtExp;
        [SerializeField] private TMP_Text txtSwipeCount;
        [SerializeField] private Image sliderExp;

        private void OnEnable()
        {
            LoadData();
            btnEditAva.onClickEvent.AddListener(OnEditAva);
            btnEditName.onClickEvent.AddListener(OnEditName);
        }

        private void OnDisable()
        {
            btnEditAva.onClickEvent.RemoveListener(OnEditAva);
            btnEditName.onClickEvent.RemoveListener(OnEditName);
        }

        private void LoadData()
        {
            var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
            imgAva.LoadSpriteAsync("ava_" + userInfo.avatarSelected);
            var data = FactoryApi.Get<ApiChatInfo>().Data;
            if (data.Info!=null && data.IsHaveProfile)
            {
                var extraData = data.Info.extra_data;
                
                txtName.text = extraData.name;
                txtLv.text = "Level " + data.Info.UserLevel;
                txtSwipeCount.text = data.Info.swipe_count.ToString();
                
                SetUserExp(data.Info);
            }
        }
        
        

        private void SetUserExp(ModelApiChatInfoDetail data)
        {
            var dataExp = FactoryApi.Get<ApiChatInfo>().Data.GetExpDisplay();
            txtExp.text = $"<color=#8A30D1>{data.exp}</color>";
            txtExp.text += $"<color=#6C6C6C>/{dataExp.total_exp_at_level}</color>";
            var sliderValue = dataExp.GetSliderValue();
            sliderExp.fillAmount = sliderValue;
        }
        
        private void OnEditAva()
        {
            this.GotoEditProfileWindow(TypeFilterPanelCustomProfile.ava_index);
        }
        
        private void OnEditName()
        {
            this.GotoEditProfileWindow(TypeFilterPanelCustomProfile.name);
        }
    }
}