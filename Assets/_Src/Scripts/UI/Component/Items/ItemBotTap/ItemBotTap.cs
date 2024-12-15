using System;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class ItemBotTap : MonoBehaviour
    {
        [SerializeField] private UIButton btnEmptyBot;
        [SerializeField] private ItemBotTapTrial itemBotTrial;
        [SerializeField] private ItemBotTapPremium itemBotPremium;

        private async void OnEnable()
        {
            var apiUser = FactoryApi.Get<ApiUser>();
            if (apiUser.Data.User==null)
                return;
            
            var apiGameInfo= FactoryApi.Get<ApiGame>().Data.Info;
            if (apiGameInfo == null)
                return;
            
            await apiUser.Get();
            OnUserChanged(apiUser.Data);
            
            btnEmptyBot.onClickEvent.AddListener(OnClickEmptyBot);
            ModelApiUser.OnChanged += OnUserChanged;
        }

        private void OnDisable()
        {
            btnEmptyBot.onClickEvent.RemoveListener(OnClickEmptyBot);
            ModelApiUser.OnChanged -= OnUserChanged;
        }

        private void OnUserChanged(ModelApiUser user)
        {
            Clear();
            var botData = user.Game.auto_bot;
            if (botData == null) return;
            
            if (botData.is_auto_bot)
            {
                itemBotPremium.SetData();
                return;
            }

            if (botData.TimerRemaining >= GameConsts.BOT_TRIAL_HARD_CHECK_TIME)
            {
                itemBotTrial.SetData(user.Game.auto_bot);
                return;
            }

            ControllerAutomation.Stop();
            btnEmptyBot.gameObject.SetActive(true);
        }

        private void Clear()
        {
            btnEmptyBot.gameObject.SetActive(false);
            itemBotTrial.gameObject.SetActive(false);
            itemBotPremium.gameObject.SetActive(false);
        }

        private void OnClickEmptyBot()
        {
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            // UnityEngine.Debug.LogError("Game Info tut: " + gameInfo.TutorialIndex);
            if (gameInfo.TutorialIndex <= (int) TutorialCategory.Undress)
            {
                if (gameInfo.current_level_girl <= 1)
                {
                    SpecialExtensionGame.PlayBotTrial();
                    return;
                }
            }
            
            this.ShowPopup(UIId.UIPopupName.PopupBotTap);
        }
    }
}