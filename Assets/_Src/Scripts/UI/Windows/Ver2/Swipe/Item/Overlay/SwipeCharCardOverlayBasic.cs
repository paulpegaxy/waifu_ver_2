using System;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.UI.Ver2.Swipe.Item;
using UnityEngine;

namespace Game.UI
{
    public class SwipeCharCardOverlayBasic : AItemSwipeCharOverlay
    {
        [SerializeField] private UIButton btnInfo;
        [SerializeField] private UIButton btnUndo;
        [SerializeField] private UIButton btnDecline;
        [SerializeField] private UIButton btnAccept;
        
        protected override void OnSetData()
        {
            
        }

        private void OnEnable()
        {
            btnInfo.onClickEvent.AddListener(OnInfo);
            btnUndo.onClickEvent.AddListener(OnUndo);
            btnDecline.onClickEvent.AddListener(OnDeclineGirl);
            btnAccept.onClickEvent.AddListener(OnAcceptGirl);
        }
     
        private void OnDisable()
        {
            btnInfo.onClickEvent.RemoveListener(OnInfo);
            btnUndo.onClickEvent.RemoveListener(OnUndo);
            btnDecline.onClickEvent.RemoveListener(OnDeclineGirl);
            btnAccept.onClickEvent.RemoveListener(OnAcceptGirl);
        }
        
        private void OnInfo()
        {
            OnSeeRateInfo?.Invoke(true);
        }
    }
}