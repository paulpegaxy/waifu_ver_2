
using BreakInfinity;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class APopupConfirmBoosterNormal : APopupConfirmBooster
    {
        [SerializeField] protected TMP_Text txtPrice;
        [SerializeField] protected Image imgIconCurrency;
        [SerializeField] protected TMP_Text txtDes;
        [SerializeField] protected ConfirmBoosterManageItem manageItemCurrent;
        [SerializeField] protected ConfirmBoosterManageItem manageItemNext;
        
        protected int NextLevel;

        protected override void OnShow()
        {
            base.OnShow();
            var dataUpgrade = FactoryApi.Get<ApiUpgrade>().Data;
            OnInit(dataUpgrade);
        }

        protected abstract void OnInit(ModelApiUpgradeInfo dataUpgrade);

        protected void ProcessTextPrice(BigDouble priceRequire)
        {
            // var currPoint = ControllerResource.Get(TypeResource.HeartPoint).Amount;
            // var visualConfig = DBM.Config.visualConfig;
            // txtPrice.color = currPoint < priceRequire
            //     ? visualConfig.GetColorStatus(TypeColor.RED)
            //     : visualConfig.GetColorStatus(TypeColor.WHITE);

            txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, priceRequire);
        }
    }
}