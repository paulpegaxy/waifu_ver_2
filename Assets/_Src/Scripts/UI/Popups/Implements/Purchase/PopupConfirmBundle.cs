using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
    public abstract class PopupConfirmBundle : MonoBehaviour
    {
        [SerializeField] private UIButton buttonBuy;

        public static Action<ModelApiShopBuy> OnCompleted;
        protected abstract void OnBuySuccess(ModelApiShopBuy data);

        protected virtual void OnBuySuccess(ModelApiShopBuyWithStar data)
        {
            
        }
        
        protected abstract void OnData(ModelApiShopData data);

        protected ModelApiShopData _data;

        protected virtual void Start()
        {
            buttonBuy.onClickEvent.AddListener(OnBuy);
        }

        protected virtual void OnDestroy()
        {
            buttonBuy.onClickEvent.RemoveListener(OnBuy);
        }

        protected virtual async void OnBuy()
        {
            if (!ControllerResource.IsEnough(TypeResource.Berry, _data.GetFinalPrice()))
            {
                Signal.Send(StreamId.UI.OpenShop);
                GetComponent<UIPopup>().Hide();
                return;
            }

            ControllerPopup.SetApiLoading(true);
            try
            {
                var apiShop = FactoryApi.Get<ApiShop>();
                var data = await apiShop.Buy(_data.id);

                OnBuySuccess(data);
                OnCompleted?.Invoke(data);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            ControllerPopup.SetApiLoading(false);
        }

        public virtual void SetData(string bundleId, int quantity = 1)
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            var data = apiShop.Data.GetItemById(bundleId);

            if (data == null) return;
            OnData(data);

            _data = data;
        }

        public void SetData(TypeShopItem type, int quantity = 1)
        {
            SetData(type.ToString().PascalToSnake(), quantity);
        }
    }
}