using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ButtonBannerBundle : MonoBehaviour
    {
        [SerializeField] private UIButton btnBundle;
        [SerializeField] private Image imgBanner;
        [SerializeField] private string eventId;
            
        private bool _isInitialized = false;

        private void Start()
        {
            if (!string.IsNullOrEmpty(eventId))
            {
                imgBanner.LoadSpriteAsync($"btn_{eventId}").Forget();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
          
            
            btnBundle.onClickEvent.AddListener(OnClick);
            if (!_isInitialized)
            {
                _isInitialized = true;
                return;
            }
            
            Refresh();
        }

        private void OnDisable()
        {
            btnBundle.onClickEvent.RemoveListener(OnClick);
        }

        private async void Refresh()
        {
            //will process with shop data
            var apiEvent = FactoryApi.Get<ApiEvent>();
            if (apiEvent.Data.EventBundleOffer == null)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnClick()
        {
            this.PostEvent(TypeGameEvent.OpenEventBundle, eventId);
            Signal.Send(StreamId.UI.OpenEventBundle);
        }
    }
}