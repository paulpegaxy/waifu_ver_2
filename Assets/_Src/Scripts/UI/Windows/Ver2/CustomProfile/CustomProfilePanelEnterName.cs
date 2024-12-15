using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class CustomProfilePanelEnterName : MonoBehaviour
    {
        [SerializeField] private UIButton btnSend;
        [SerializeField] private UIButton btnSave;
        [SerializeField] private TMP_InputField ipfEnterName;
        
        public static Action<string> OnSendName;
        private bool _isFirstOpen;

        private void OnEnable()
        {
            var info = FactoryApi.Get<ApiChatInfo>().Data;
            if (info.Info == null)
                return;
            
            btnSave.gameObject.SetActive(info.IsHaveProfile);
            btnSend.gameObject.SetActive(!info.IsHaveProfile);
            
            ipfEnterName.text = info.IsHaveProfile ? info.Info.extra_data.name : "";
            btnSend.onClickEvent.AddListener(OnSend);
            btnSave.onClickEvent.AddListener(OnSaveName);
        }

        private void OnDisable()
        {
            btnSend.onClickEvent.RemoveListener(OnSend);
            btnSave.onClickEvent.RemoveListener(OnSaveName);
        }

        private void OnSend()
        {
            if (string.IsNullOrEmpty(ipfEnterName.text))
            {
                ControllerPopup.ShowToastError("Please enter your name");
                return;
            }
            OnSendName?.Invoke(ipfEnterName.text);
        }

        private void OnSaveName()
        {
            if (string.IsNullOrEmpty(ipfEnterName.text))
            {
                ControllerPopup.ShowToastError("Please enter your name");
                return;
            }

            bool isModified = ipfEnterName.text.Equals(FactoryApi.Get<ApiChatInfo>().Data.Info.extra_data.name) ==
                              false;
            if (isModified)
            {
                this.SaveProfile(TypeFilterPanelCustomProfile.name, ipfEnterName.text);
            }
            else
                this.SaveProfile(TypeFilterPanelCustomProfile.name);
        }
    }
}