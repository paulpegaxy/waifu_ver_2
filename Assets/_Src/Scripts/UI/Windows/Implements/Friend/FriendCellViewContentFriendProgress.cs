// Author: ad   -
// Created: 22/09/2024  : : 16:09
// DateUpdate: 22/09/2024

using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Triggers.Internal;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FriendCellViewContentFriendProgress : ESCellView<ModelFriendCellView>
    {
        [SerializeField] private TMP_Text textName;
        [SerializeField] private ItemAvatar itemAvatar;
        [SerializeField] private TMP_Text txtBtn;
        [SerializeField] private TMP_Text textFriendCount;
        [SerializeField] private GameObject objPremiumUser;
        [SerializeField] private FriendItemProgressBar itemProgress;
        [SerializeField] private UIButton btnSlap;
        [SerializeField] private GameObject objTryTomorrow;

        private int _friendId;

        public static Action OnRefreshAfterSlap;

        private void OnEnable()
        {
            btnSlap.onClickEvent.AddListener(OnSlapFriend);
        }

        private void OnDisable()
        {
            btnSlap.onClickEvent.RemoveListener(OnSlapFriend);
        }

        public override void SetData(ModelFriendCellView model)
        {
            var data = model as ModelFriendCellViewContentFriendProgress;
            if (data==null)
                return;

            _friendId = data.FriendId;
   
            textName.text = data.Name.Truncate(GameConsts.MAX_LENGTH_NICK);
            itemAvatar.SetNameAvatar(data.Name);
            textFriendCount.text = $"+{data.FriendCount} {Localization.Get(TextId.Common_Friends)}";
            objPremiumUser.SetActive(data.IsPremiumUser);
            bool canSlap = data.DelaySlapTime <= 0;
            btnSlap.gameObject.SetActive(canSlap);
            objTryTomorrow.SetActive(!canSlap);
            ProcessProgress(data);
            
        }

        private void ProcessProgress(ModelFriendCellViewContentFriendProgress data)
        {
            List<DataFriendItemProgress> listData = new List<DataFriendItemProgress>();
            int count = data.ConfigProgress.Count;

            var currChar = DBM.Config.rankingConfig.GetDataBasedCurrentGirlLevel(data.CurrentGirlLevel);
            
            int progress = 1;
            for (int i = 0; i < count; i++)
            {
                var ele = data.ConfigProgress[i];
                bool isClaimed = currChar.type >= ele.league;
                if (isClaimed)
                    progress++;
                
                listData.Add(new DataFriendItemProgress()
                {
                    value = ele.GetFinalBonus(data.IsPremiumUser),
                    isClaimed = isClaimed,
                    typeChar = ele.league
                });
                
            }

            // UnityEngine.Debug.LogError("USer: " + data.Name + ",. progress: " + progress + ", max : " + count);
            itemProgress.LoadData(listData, progress);
        }

        private async void OnSlapFriend()
        {
            this.ShowProcessing();
            try
            {
                var apiFriend = FactoryApi.Get<ApiFriend>();
                await apiFriend.PostSlapMyFriend(_friendId);
                ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Friend_DoneSlapAss));
                OnRefreshAfterSlap?.Invoke();
                this.HideProcessing();
            }
            catch (Exception e)
            {
                e.ShowError();
            }

        }
    }
}