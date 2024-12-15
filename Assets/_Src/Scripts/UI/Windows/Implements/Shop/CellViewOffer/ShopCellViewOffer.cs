using System;
using UnityEngine;
using Game.Model;

namespace Game.UI
{
    public class ShopCellViewOffer : ESCellView<ModelShopOfferCellView>
    {
        protected ModelApiShopData Data;

        public static Action OnBuySuccess;

        public override void SetData(ModelShopOfferCellView data)
        {
            SetData(data.ShopItem);
        }
        
        protected virtual void SetData(ModelApiShopData data)
        {
            Data = data;
        }
        
        protected void InvokeBuySuccess()
        {
            OnBuySuccess?.Invoke();
        }
        
        public virtual void SetDirectlyData<T>(ModelApiShopData data,Action callBack,T param)
        {
            SetData(data);
        }
    }
}