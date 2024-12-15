using System;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public abstract class UIWindow : MonoBehaviour
    {
        public static Action<UIId.UIViewCategory, UIId.UIViewName> OnDataLoaded;

        private bool _isInitialized = false;

        private void OnEnable()
        {
            // var apiGameInfo = FactoryApi.Get<ApiGame>();
            // if (apiGameInfo.Data.Info==null)
            //     return;
            if (!_isInitialized)
            {
                _isInitialized = true;
                return;
            }
            OnEnabled();
        }

        private void OnDisable()
        {
            OnDisabled();
        }

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }
    }
}