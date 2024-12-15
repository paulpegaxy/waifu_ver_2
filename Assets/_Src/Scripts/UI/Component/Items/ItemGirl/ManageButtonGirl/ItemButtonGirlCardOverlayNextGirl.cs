// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemButtonGirlCardOverlayNextGirl : AItemButtonGirlCardOverlayNormal
    {
        
        protected override void OnSetData()
        {
            OnReloadInfo(FactoryApi.Get<ApiGame>().Data.Info);
        }
        
        protected override void OnReloadInfo(ModelApiGameInfo data)
        {
            btnClick.gameObject.SetActive(true);
            var cost = data.next_girl_level_data.CostParse;
            txtPrice.text = cost.ToLetter();
            txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, cost, TypeColor.PINK);

            bool isEnough = data.PointParse >= cost;
            btnClick.GetComponent<Image>().material = isEnough ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
            btnClick.interactable = isEnough;
            
            // if (isEnough)
            // {
            //     if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.NextGirl))
            //     {
            //         if (ControllerResource.IsEnough(TypeResource.HeartPoint, cost))
            //         {
            //             // UnityEngine.Debug.LogError("Vao active ne: ");
            //             FactoryApi.Get<ApiEvent>().Get().Forget();
            //             TutorialMgr.Instance.ActiveTutorial(TutorialCategory.NextGirl);
            //         }
            //     }
            // }
        }

       
        
         protected override async void OnClick()
        {
             var apiGame = FactoryApi.Get<ApiGame>();
            async void ProcessNextGirlAfterConfirm(UIPopup popup = null)
            {
                this.ShowProcessing();
                var nextGirlData = DBM.Config.rankingConfig.GetDataBasedCurrentGirlLevel(apiGame.Data.Info.current_level_girl + 1);
                var go = AnR.Get<GameObject>($"{nextGirlData.girlId}_spine");
                if (go == null)
                {
                    await AnR.LoadAddressableByLabels<Texture>(new List<string>() { nextGirlData.girlId.ToString() });
                    await AnR.LoadAddressableByLabels<GameObject>(new List<string>() { nextGirlData.girlId.ToString() });
                }
                
                SpecialExtensionGame.SaveNextGirl(nextGirlData.girlId);

                await apiGame.PostIncreaseLevel();
                await FactoryApi.Get<ApiEvent>().Get();
                
                // this.ShowPopup(UIId.UIPopupName.PopupGirlReward);
                this.PostEvent(TypeGameEvent.NextGirlSuccess);
                if (popup != null)
                    popup.Hide();
                
                this.HideProcessing();
            }

            if (apiGame.Data.Info.current_level_girl < GameConsts.MAX_LEVEL_PER_CHAR)
            {
                btnClick.gameObject.SetActive(false);
                ProcessNextGirlAfterConfirm();
                return;
            }

            ControllerPopup.ShowConfirm(Localization.Get(TextId.Confirm_NextGirl), onOk: (popup) =>
            {
                if (ServiceLocator.GetService<IServiceValidate>().ValidateNextGirl())
                {
                    btnClick.gameObject.SetActive(false);
                    ProcessNextGirlAfterConfirm(popup);
                }
                else
                    popup.Hide();
            });
        }
    }
}