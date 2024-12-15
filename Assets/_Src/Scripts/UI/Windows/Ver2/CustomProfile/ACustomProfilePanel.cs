using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Slime.UI;
using UnityEngine;

namespace Game.UI
{
    public abstract class ACustomProfilePanel : BasePanel
    {
        [SerializeField] private TypeFilterPanelCustomProfile typePanel;
        [SerializeField] private UIButton btnBack;
        [SerializeField] private UIButton btnNext;
        [SerializeField] private UIButton btnSave;

        public TypeFilterPanelCustomProfile TypePanel => typePanel;
        public string KeyProfile;
        

        public static Action<TypeFilterPanelCustomProfile> OnNext;
        public static Action<TypeFilterPanelCustomProfile> OnBack;

        protected bool IsModifiedProfile;
        protected ModelApiChatInfoDetail Data => FactoryApi.Get<ApiChatInfo>().Data.Info;

        protected virtual void OnEnable()
        {
            IsModifiedProfile = false;
            LoadData();
            CheckHaveProfile();
            btnNext.onClickEvent.AddListener(OnClickNext);
            btnBack.onClickEvent.AddListener(OnClickBack);
            btnSave.onClickEvent.AddListener(OnClickSave);
        }

        protected virtual void OnDisable()
        {
            btnNext.onClickEvent.RemoveListener(OnClickNext);
            btnBack.onClickEvent.RemoveListener(OnClickBack);
            btnSave.onClickEvent.RemoveListener(OnClickSave);
        }

        protected virtual async void OnClickNext()
        {
            await OnProcessAction();   
            OnNext?.Invoke(typePanel);
        }

        private void LoadData()
        {
            OnLoadData();
        }

        private void CheckHaveProfile()
        {
            var info = FactoryApi.Get<ApiChatInfo>().Data;
            if (info.IsHaveProfile)
            {
                TurnOnBtnSave(true);
            }
            else
            {
                TurnOnBtnSave(false);
            }
        }

        protected abstract void OnLoadData();

        protected abstract void OnSaveData();

        private void OnClickSave()
        {
            OnSaveData();
            OnProcessAction();
            if (typePanel == TypeFilterPanelCustomProfile.ava_index)
            {
                SpecialExtensionVer2.SaveProfileAvatar(KeyProfile);
            }
            else
            {
                if (IsModifiedProfile)
                {
                    this.SaveProfile(typePanel, KeyProfile);
                }
                else
                {
                    this.SaveProfile(typePanel);
                }
            }
        }

        protected virtual void OnClickBack()
        {
            OnBack?.Invoke(typePanel);
        }

        protected virtual async UniTask OnProcessAction()
        {
        }
        
        private void TurnOnBtnSave(bool isOn)
        {
            btnSave.gameObject.SetActive(isOn);
            btnBack.gameObject.SetActive(!isOn);
            btnNext.gameObject.SetActive(!isOn);
        }
    }

    public enum TypeFilterPanelCustomProfile
    {
        name,
        interested_in,
        zodiac,
        genres,
        ava_index
    }
}