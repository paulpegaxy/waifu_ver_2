using System;
using System.Collections.Generic;
using _Src.Scripts.Data.DBM.Configs;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class CustomProfilePanelSettingVisual : ACustomProfilePanel
    {
        [SerializeField] private UIToggleGroup tggVisual;
        [SerializeField] private Transform posContain;
        
        private int _toggleIndex;
        
        protected override async UniTask OnFetchData()
        {
        }

        private void Start()
        {
            int count = GameConsts.MAX_AVATAR_USER;
            List<int> list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
            posContain.FillData<int, CustomProfileItemVisual>(list, (data, view, index) =>
            {
                view.SetData(data);
                view.GetComponent<UIToggle>().isOn = false;
            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            tggVisual.OnToggleTriggeredCallback.AddListener(OnToggle);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            tggVisual.OnToggleTriggeredCallback.RemoveListener(OnToggle);
        }
        
        private void OnToggle(UIToggle toggle)
        {
            var lastIndex = tggVisual.lastToggleOnIndex;
            _toggleIndex = lastIndex;
        }

        protected override void OnLoadData()
        {
            
        }

        protected override void OnSaveData()
        {
            
        }

        protected override async UniTask OnProcessAction()
        {
            KeyProfile = "ava_" + (_toggleIndex);
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            if (userInfo.avatarSelected!=_toggleIndex)
            {
                userInfo.avatarSelected = _toggleIndex;
                storageUserInfo.Save();
            }
        }
    }
}