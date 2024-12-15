// Author: ad   -
// Created: 17/10/2024  : : 02:10
// DateUpdate: 17/10/2024

using System;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class ItemCheckQuestFollowX : AItemCheckStartGame
    {
        [SerializeField] private GameObject objQuestFollowX;
        [SerializeField] private UIButton btnGoto;

        protected override void Awake()
        {
            base.Awake();
            objQuestFollowX.SetActive(false);
        }

        protected override void OnEnabled()
        {
            Load();
            btnGoto.onClickEvent.AddListener(OnClickGoto);
        }
        
        protected override void OnDisabled()
        {
            btnGoto.onClickEvent.RemoveListener(OnClickGoto);
        }

        protected override void OnInit()
        {
        }

        private void Load()
        {
            if (FactoryApi.Get<ApiGame>().Data.Info==null)
                return;
            
            if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.GameFeature))
            {
                return;
            }
            
            var apiQuest = FactoryApi.Get<ApiQuest>();
            objQuestFollowX.SetActive(!apiQuest.Data.IsDoneFollowXQuest());
        }

        private async void OnClickGoto()
        {
            var apiQuest = FactoryApi.Get<ApiQuest>();
            objQuestFollowX.SetActive(false);
            await SpecialExtensionGame.CheckFollowXQuest(apiQuest.Data.QuestFollowXData);
            Signal.Send(StreamId.UI.OpenQuest);
        }
    }
}