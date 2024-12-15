using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.UI.Ver2.Swipe.Item;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SwipeCharCardOverlayOutOfSwipe : AItemSwipeCharOverlay
    {
        [SerializeField] private UIButton btnInfo;
        [SerializeField] private UIButton btnUnlockSwipeMore;
        [SerializeField] private TMP_Text txtLimitSwipeInfo;
        
        protected override void OnSetData()
        {
            ProcessLimitSwipeInfo();
        }
        
        private void OnEnable()
        {
            btnInfo.onClickEvent.AddListener(OnInfo);
            btnUnlockSwipeMore.onClickEvent.AddListener(OnUnlockSwipeMore);
        }
        
        private void OnDisable()
        {
            btnInfo.onClickEvent.RemoveListener(OnInfo);
            btnUnlockSwipeMore.onClickEvent.RemoveListener(OnUnlockSwipeMore);
        }
        
        private void ProcessLimitSwipeInfo()
        {
            string content = "24 hours";
            txtLimitSwipeInfo.text = $"<color=white>Out of swiptes </color>{content}<color=white> until reset</color>";
        }
        
        private void OnInfo()
        {
            OnSeeRateInfo?.Invoke(true);
        }
        
        private void OnUnlockSwipeMore()
        {
            ControllerPopup.ShowToastComingSoon();
        }
    }
}