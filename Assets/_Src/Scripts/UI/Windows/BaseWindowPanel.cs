using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public abstract class BaseWindowPanel<TEnum,TData> : MonoBehaviour where TEnum : Enum
    {
        protected List<TData> ListData;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public async void Show(TEnum type)
        {
            if (!IsThisPanel(type))
            {
                Hide();
                return;
            }

            gameObject.SetActive(true);
            ListData = new List<TData>();
            await OnLoadData();
            SetData();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        protected abstract bool IsThisPanel(TEnum type);
        protected abstract UniTask OnLoadData();
        protected abstract void SetData();
    }
}