// Author: ad   -
// Created: 22/09/2024  : : 20:09
// DateUpdate: 22/09/2024

using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class FriendItemProgress : ItemProgressElement<DataFriendItemProgress>
    {
        [SerializeField] private ItemAvatar itemAvatar;
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private GameObject objValueBar;
        [SerializeField] private GameObject objClaimed;
        
        protected override void OnSetData(DataFriendItemProgress data)
        {
            objClaimed.SetActive(data.isClaimed);
            objValueBar.SetActive(!data.isClaimed);
            txtValue.text = "+" + data.value.ToString("#,##0");
            var girlData = DBM.Config.rankingConfig.GetRankData(data.typeChar);
            if (girlData==null)
            {
                UnityEngine.Debug.LogError("Null me roi");
                return;
            }
            itemAvatar.SetImageAvatar(girlData.girlId);
        }
    }
}