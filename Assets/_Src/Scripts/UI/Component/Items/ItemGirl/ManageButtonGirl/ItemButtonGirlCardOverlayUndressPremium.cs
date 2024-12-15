// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemButtonGirlCardOverlayUndressPremium : AItemButtonGirlCardOverlayPremium
    {
        [SerializeField] private TMP_Text txtPrice;
        [SerializeField] private TMP_Text txtProfit;

        private ModelApiUpgradePremiumChar _charData;
        
        protected override void OnSetData()
        {
            _charData = FactoryApi.Get<ApiUpgrade>().Data.GetPremiumChar(Data.girlId);
            txtPrice.text = _charData.next.CostParse.ToLetter();
            txtProfit.text = $"+{_charData.PointProfitParse.ToLetter()} SUGAR/h";

            var cost = _charData.next.CostParse;
            txtPrice.text = cost.ToLetter();
            txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, cost, TypeColor.PINK);

            bool isEnough = FactoryApi.Get<ApiGame>().Data.Info.PointParse >= cost;

            btnClick.GetComponent<Image>().material = isEnough ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
            btnClick.interactable = isEnough;
        }

        protected override async void OnClick()
        {
            try
            {
                await FactoryApi.Get<ApiUpgrade>().PostUpgradePremiumWaifu(_charData.id);
                FactoryApi.Get<ApiGame>().GetInfo().Forget();
                this.PostEvent(TypeGameEvent.UndressGirl);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }
    }
}