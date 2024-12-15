using System;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Template.Defines;
using UnityEngine;
using WebGLSupport;
using WebGLInput = WebGLSupport.WebGLInput;

namespace Game.UI
{
    public class PopupProtectInput : MonoBehaviour
    {
        [SerializeField] private UIButton btnClose;
        
        private void OnEnable()
        {
            btnClose.onClickEvent.AddListener(OnClose);
            WebGLInput.OnHideInput += OnHideInput;
        }

        private void OnDisable()
        {
            btnClose.onClickEvent.RemoveListener(OnClose);
            WebGLInput.OnHideInput -= OnHideInput;
        }

        private void OnHideInput()
        {
            GetComponent<UIPopup>().InstantHide();
        }

        private void OnClose()
        {
            // WebGLWindow.ForceResize();
            if (TelegramWebApp.Platform() == "ios")
            {
                WebGLWindow.ForceBlur(false);
            }
            else
            {
                WebGLWindow.ForceBlur(true);
            }

            GetComponent<UIPopup>().InstantHide();
        }
    }
}