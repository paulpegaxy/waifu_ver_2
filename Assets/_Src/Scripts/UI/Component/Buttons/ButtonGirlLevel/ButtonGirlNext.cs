using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ButtonGirlNext : AButtonGirlLevel
    {
        protected override void OnReloadInfo(ModelApiGameInfo data)
        {
            bool isShow = ServiceValidate.CanNextGirl();
            btnClick.gameObject.SetActive(isShow);
            if (isShow)
            {
                var cost = data.next_girl_level_data.CostParse;
                txtPrice.text = cost.ToLetter();
                txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, cost, TypeColor.PINK);

                bool isEnough = data.PointParse >= cost;
                btnClick.GetComponent<Image>().material = isEnough ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
                btnClick.enabled = isEnough;
                // if (isEnough)
                // {
                //     if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.NextGirl))
                //     {
                //         if (ControllerResource.IsEnough(TypeResource.HeartPoint, cost))
                //         {
                //             FactoryApi.Get<ApiEvent>().Get().Forget();
                //             TutorialMgr.Instance.ActiveTutorial(TutorialCategory.NextGirl);
                //         }
                //     }
                // }
            }
        }

        protected override void OnClickButton()
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
                
                var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
                var userInfo = storageUserInfo.Get();
                userInfo.isChoosePremiumWaifu = false;
                userInfo.selectedWaifuId = nextGirlData.girlId;
                storageUserInfo.Save();

                await apiGame.PostIncreaseLevel();
                await FactoryApi.Get<ApiEvent>().Get();
                if (popup != null)
                    popup.Hide();
                
                // this.ShowPopup(UIId.UIPopupName.PopupGirlReward);
                this.PostEvent(TypeGameEvent.NextGirlSuccess);
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