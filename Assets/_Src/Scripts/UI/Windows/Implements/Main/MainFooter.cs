using System;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using DG.Tweening;
using Doozy.Runtime.Signals;
using Game.Model;
using Game.Runtime;
using Game.Core;
using Game.Extensions;
using Sirenix.Utilities;
using Template.Defines;

namespace Game.UI
{
    public class MainFooter : GameplayListener
    {
        [SerializeField] private UIButton btnFriend;
        [SerializeField] private UIButton btnQuest;
        [SerializeField] private UIButton btnGift;
        [SerializeField] private UIButton btnIdleEarn;
        
        [SerializeField] private UIButton btnBooster;
        [SerializeField] private UIButton btnGallery;
        [SerializeField] private UIButton btnInbox;
        [SerializeField] private UIButton btnMessage;

        [SerializeField] private GameObject objNotifyInbox;
        [SerializeField] private GameObject objHolderNotifyInbox;

        [SerializeField] private TMP_Text txtBubbleMessage;

        [SerializeField] private GameObject[] arrEffectBtn;


        private ModelStorageSetting _setting;
        private int _currentGirlIdForMessage;
        
        private void Awake()
        {
            arrEffectBtn.ForEach(x => x.gameObject.SetActive(true));
            TurnOnNotifyMessage(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            btnGift.onClickEvent.AddListener(OnClickGift);
            btnInbox.onClickEvent.AddListener(OnInbox);
            btnIdleEarn.onClickEvent.AddListener(OnClickIdleEarn);
            btnMessage.onClickEvent.AddListener(OnClickMessage);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            btnGift.onClickEvent.RemoveListener(OnClickGift);
            btnInbox.onClickEvent.RemoveListener(OnInbox);
            btnIdleEarn.onClickEvent.RemoveListener(OnClickIdleEarn);
            btnMessage.onClickEvent.RemoveListener(OnClickMessage);
        }

        protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            var dictMessage = storageSetting.Get().dictLvAlreadyReadMessage;
            if (dictMessage == null) return;

            if (!SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.Undress))
            {
                TurnOnNotifyMessage(false);
                return;
            }

            foreach (var item in dictMessage)
            {
                if (!item.Value)
                {
                    _currentGirlIdForMessage = item.Key;
                    TurnOnNotifyMessage(true);
                    ShowMessage(gameInfo);
                    return;
                }
            }

            if (dictMessage.TryGetValue(gameInfo.CurrentGirlId,out bool isRead) && isRead)
            {
                TurnOnNotifyMessage(false);
            }
            else
            {
                _currentGirlIdForMessage = gameInfo.CurrentGirlId;
                dictMessage.TryAdd(gameInfo.CurrentGirlId, false);
                ShowMessage(gameInfo);
                TurnOnNotifyMessage(true);
            }
        }

        private void ShowMessage(ModelApiGameInfo gameInfo)
        {
            int modLevel = gameInfo.current_level_girl % GameConsts.MAX_LEVEL_PER_CHAR;
            var messageRank = ExtensionEnum.ToMessage(gameInfo.CurrentGirlId, modLevel);
            txtBubbleMessage.text = messageRank.Truncate(GameConsts.MAX_LENGHT_SHORT_MESSAGE);
        }

        private void TurnOnNotifyMessage(bool isOn)
        {
            objNotifyInbox.SetActive(isOn);
            objHolderNotifyInbox.SetActive(isOn);
        }
        
        private void OnClickGift()
        {
            ControllerPopup.ShowToastComingSoon();
        }

        private void OnInbox()
        {
            var popup = this.ShowPopup<PopupChat>(UIId.UIPopupName.PopupChat);
            popup.Show();
        }
        
        private void OnClickIdleEarn()
        {
            Signal.Send(StreamId.UI.OpenUpgrade);
        }

        private void OnClickMessage()
        {
            var popup = this.ShowPopup<PopupChat>(UIId.UIPopupName.PopupChat);
            popup.Show(_currentGirlIdForMessage);
        }
    }
}