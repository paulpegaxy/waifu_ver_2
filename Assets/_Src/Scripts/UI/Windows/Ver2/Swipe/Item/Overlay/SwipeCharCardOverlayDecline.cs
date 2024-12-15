using Doozy.Runtime.UIManager.Components;
using Game.UI.Ver2.Swipe.Item;
using UnityEngine;

namespace Game.UI
{
    public class SwipeCharCardOverlayDecline : AItemSwipeCharOverlay
    {
        [SerializeField] private UIButton btnAccept;
        [SerializeField] private UIButton btnUndo;
        
        protected override void OnSetData()
        {
            
        }
        
        private void OnEnable()
        {
            btnAccept.onClickEvent.AddListener(OnAcceptGirl);
            btnUndo.onClickEvent.AddListener(OnUndo);
        }
        
        private void OnDisable()
        {
            btnAccept.onClickEvent.RemoveListener(OnAcceptGirl);
            btnUndo.onClickEvent.RemoveListener(OnUndo);
        }
    }
}