// Author: ad   -
// Created: 14/12/2024  : : 20:12
// DateUpdate: 14/12/2024

using Doozy.Runtime.UIManager.Components;
using Game.Defines;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public abstract class AUserProfileCellView : MonoBehaviour
    {
        [SerializeField] private TypeFilterPanelCustomProfile type;
        [SerializeField] private UIButton btnClick;
        
        private void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
            LoadData();
        }
        
        private void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
        }

        private void LoadData()
        {
            var data = FactoryApi.Get<ApiChatInfo>().Data;
            if (data.Info == null) return;
            if (data.IsHaveProfile)
            {
                OnLoadData(data.Info.extra_data);
            }
        }

        protected abstract void OnLoadData(ModelApiChatInfoExtra data);

        private void OnClick()
        {
            this.GotoEditProfileWindow(type);
        }
    }
}