// Author: ad   -
// Created: 21/09/2024  : : 23:09
// DateUpdate: 21/09/2024

using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class AButtonBundleShop : AButtonItemPackage
    {
        [SerializeField] private UIButton btnClick;

        private void Awake()
        {
            btnClick.gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
            ModelApiShop.OnChanged += OnChangedPackageShop;
        }

        protected virtual void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
            ModelApiShop.OnChanged -= OnChangedPackageShop;
        }

        protected override void OnInit()
        {
            Refresh(FactoryApi.Get<ApiShop>().Data);
        }

        private void OnChangedPackageShop(ModelApiShop data)
        {
            Refresh(data);
        }

        private void Refresh(ModelApiShop data)
        {
            if (data.Shop?.Count <= 0)
                return;

            btnClick.gameObject.SetActive(OnValidateEvent(data));
        }

        protected abstract bool OnValidateEvent(ModelApiShop data);

        protected abstract void OnClick();
    }
}