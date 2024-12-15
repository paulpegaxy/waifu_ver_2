// Author: ad   -
// Created: 01/12/2024  : : 02:12
// DateUpdate: 01/12/2024

using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.UI.Ver2.Swipe.Item;
using UnityEngine;

namespace Game.UI
{
    public class SwipeCharCardOverlayAccept : AItemSwipeCharOverlay
    {
        [SerializeField] private UIButton btnGotoChat;
        [SerializeField] private UIButton btnDecline;
        [SerializeField] private UIButton btnUndo;
        
        protected override void OnSetData()
        {
            
        }
        
        private void OnEnable()
        {
            btnGotoChat.onClickEvent.AddListener(OnGotoChat);
            btnDecline.onClickEvent.AddListener(OnDecline);
            btnUndo.onClickEvent.AddListener(OnUndo);
        }
        
        private void OnDisable()
        {
            btnGotoChat.onClickEvent.RemoveListener(OnGotoChat);
            btnDecline.onClickEvent.RemoveListener(OnDecline);
            btnUndo.onClickEvent.RemoveListener(OnUndo);
        }
        
        private void OnDecline()
        {
            ControllerPopup.ShowConfirm("Are you sure you want to un match this girl?", onOk: (popup)=>
            {
                OnDeclineGirl();
                popup.Hide();
            });
        }
        
        private void OnGotoChat()
        {
            var popup = this.ShowPopup<PopupSuccessMatch>(UIId.UIPopupName.PopupSuccessMatch);
            popup.SetData(Data.entityConfig);
        }
    }
}