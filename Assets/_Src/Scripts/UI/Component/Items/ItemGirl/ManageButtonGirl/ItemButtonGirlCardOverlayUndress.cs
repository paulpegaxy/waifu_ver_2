// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemButtonGirlCardOverlayUndress : AItemButtonGirlCardOverlayNormal
    {
        protected override void OnSetData()
        {
            OnReloadInfo(FactoryApi.Get<ApiGame>().Data.Info);
        }

        // protected override void OnEnable()
        // {
        //     base.OnEnable();

        // }

        protected override void OnReloadInfo(ModelApiGameInfo data)
        {
            var status = this.GetEventData<TypeGameEvent, bool>(TypeGameEvent.ActiveTutorialUndress);
            if (status)
            {
                LoadData(FactoryApi.Get<ApiGame>().Data.Info);
                TutorialMgr.Instance.ActiveTutorial(TutorialCategory.Undress);
                return;
            }
            if (!ShouldShowButton(data))
            {
                // UnityEngine.Debug.Log("Tai sao lai vao day");
                btnClick.gameObject.SetActive(false);
                return;
            }

            LoadData(data);


        }

        protected override void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
        {
            if (category == TutorialCategory.Undress && data.State == TutorialState.Undress)
            {
                var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
                if (gameInfo.PointParse >= GameConsts.INIT_FIRST_UNDRESS)
                {
                    // LoadData(FactoryApi.Get<ApiGame>().Data.Info);
                }
            }
        }

        private void LoadData(ModelApiGameInfo data)
        {

            // UnityEngine.Debug.LogError("Vao day ne");
            btnClick.gameObject.SetActive(true);
            var cost = data.next_girl_level_data.CostParse;
            txtPrice.text = cost.ToLetter();
            txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, cost, TypeColor.PINK);

            bool isEnough = data.PointParse >= cost;

            btnClick.GetComponent<Image>().material = isEnough ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
            btnClick.interactable = isEnough;
        }

        private bool ShouldShowButton(ModelApiGameInfo data)
        {
            if (data.current_level_girl <= 0)
            {
                return false;
            }

            if (data.TutorialIndex < (int)TutorialCategory.Upgrade)
                return false;

            return data.TutorialIndex >= (int)TutorialCategory.Undress ||
                   TutorialMgr.Instance.GetTutorialState(TutorialCategory.Undress) >= TutorialState.Undress;
        }

        protected override async void OnClick()
        {
            var apiGame = FactoryApi.Get<ApiGame>();

            if (ServiceLocator.GetService<IServiceValidate>().ValidateUndress())
            {
                this.ShowProcessing();
                try
                {
                    btnClick.interactable = false;
                    await apiGame.PostIncreaseLevel();
                    if (apiGame.Data.Info.IsNeedProtectGirl() && apiGame.Data.Info.CurrentGirlId != 20009)
                    {
                        this.ShowPopup(UIId.UIPopupName.PopupProtectGirl);
                    }
                    apiGame.Data.Info.Notification();
                    FactoryApi.Get<ApiEvent>().Get().Forget();

                    SpecialExtensionShop.RecheckOffer().Forget();

                    this.HideProcessing();
                    this.PostEvent(TypeGameEvent.UndressGirl);
                }
                catch (Exception e)
                {
                    btnClick.interactable = true;
                    e.ShowError();
                }
            }
        }
    }
}