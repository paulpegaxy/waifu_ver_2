using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine.UI;

namespace Game.UI
{
    public class ButtonGirlUndress : AButtonGirlLevel
    {
        protected override void OnReloadInfo(ModelApiGameInfo data)
        {
            if (!ShouldShowButton(data))
                return;

            LoadData(data);
        }

        protected override void OnStepEnter(TutorialCategory category, ModelTutorialStep data)
        {
            if (category == TutorialCategory.Undress && data.State == TutorialState.Undress)
            {
                var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
                if (gameInfo.PointParse >= GameConsts.INIT_FIRST_UNDRESS)
                {
                    LoadData(FactoryApi.Get<ApiGame>().Data.Info);
                }
            }
        }

        private void LoadData(ModelApiGameInfo data)
        {
            bool isShow = ServiceValidate.CanUndress(data);
            btnClick.gameObject.SetActive(isShow);

            if (!isShow) return;

            var cost = data.next_girl_level_data.CostParse;
            txtPrice.text = cost.ToLetter();
            txtPrice.color = SpecialExtensionGame.GetColorTextPrice(TypeResource.HeartPoint, cost, TypeColor.PINK);

            bool isEnough = data.PointParse >= cost;

            btnClick.GetComponent<Image>().material = isEnough ? null : DBM.Config.visualConfig.materialConfig.matDisableObject;
            btnClick.interactable = isEnough;
        }

        private bool ShouldShowButton(ModelApiGameInfo data)
        {
            if (data.current_level_girl <= 0 && data.TutorialIndex < (int)TutorialCategory.Upgrade)
                return false;

            return data.TutorialIndex >= (int)TutorialCategory.Undress ||
               TutorialMgr.Instance.GetTutorialState(TutorialCategory.Undress) >= TutorialState.Undress;
        }

        protected override async void OnClickButton()
        {
            var apiGame = FactoryApi.Get<ApiGame>();

            if (ServiceLocator.GetService<IServiceValidate>().ValidateUndress())
            {
                
                try
                {
                    btnClick.interactable = false;
                    await apiGame.PostIncreaseLevel();
                    apiGame.Data.Info.Notification();
                    FactoryApi.Get<ApiEvent>().Get().Forget();

                    SpecialExtensionShop.RecheckOffer().Forget();

                    this.PostEvent(TypeGameEvent.UndressGirl);
                }
                catch (Exception e)
                {
                    e.ShowError();
                }
            }
        }
    }
}