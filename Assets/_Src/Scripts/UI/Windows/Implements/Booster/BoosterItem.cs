// Author:    -    ad
// Created: 28/07/2024  : : 5:20 PM
// DateUpdate: 28/07/2024

using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class BoosterItem : MonoBehaviour
    {
        [SerializeField] protected TMP_Text txtName;
        [SerializeField] protected TMP_Text txtLevel;
        [SerializeField] protected TMP_Text txtGoldNeed;
        [SerializeField] protected Image imgIcon;
        [SerializeField] protected Image imgCurrency;
        [SerializeField] protected UIButton btnUpgrade;

        protected ModelApiUpgradeInfo UpgradeInfo;

        private void Awake()
        {
            btnUpgrade.onClickEvent.AddListener(OnClickUpgrade);
        }

        private void OnDestroy()
        {
            btnUpgrade.onClickEvent.RemoveListener(OnClickUpgrade);
        }
        
        public void LoadData()
        {
            UpgradeInfo = FactoryApi.Get<ApiUpgrade>().Data;
            OnLoadData();
        }

        protected abstract void OnLoadData();

        protected abstract void OnClickUpgrade();
    }
}