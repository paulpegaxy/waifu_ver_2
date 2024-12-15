// Author: ad   -
// Created: 21/09/2024  : : 21:09
// DateUpdate: 21/09/2024

using System;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(UIButton))]
    class ButtonMainWindowAction : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private MainWindowAction action;
        [HideInInspector] public string actionString;

        public static Action<MainWindowAction> OnAction;
        public MainWindowAction Action => action;

        private void Awake()
        {
            GetComponent<UIButton>().onClickEvent.AddListener(OnActionClick);
        }

        private void OnDestroy()
        {
            GetComponent<UIButton>().onClickEvent.RemoveListener(OnActionClick);
        }

        private void OnActionClick()
        {
            OnAction?.Invoke(action);
        }

        public void OnBeforeSerialize()
        {
            actionString = action.ToString();
        }

        public void OnAfterDeserialize()
        {
            action = (MainWindowAction)Enum.Parse(typeof(MainWindowAction), actionString);
        }
    }

    public enum MainWindowAction
    {
        None,
        EventMeetYuki,
        PartnerMergePal,
        LimitedOffer,
        Offer24Hour,
        PartnerEtaku,
        PartnerWaifuPride,
        Jackpot,
        PartnerYggPlay
    }
}