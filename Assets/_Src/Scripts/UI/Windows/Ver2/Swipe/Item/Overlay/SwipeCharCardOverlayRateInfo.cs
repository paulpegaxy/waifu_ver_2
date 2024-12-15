using Doozy.Runtime.UIManager.Components;
using Game.UI.Ver2;
using Game.UI.Ver2.Swipe.Item;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SwipeCharCardOverlayRateInfo : AItemSwipeCharOverlay
    {
        [SerializeField] private UIButton btnBack;
        [SerializeField] private TMP_Text txtRate;
        
        protected override void OnSetData()
        {
            txtRate.text = Data.entityConfig.GetPercentMatchRateString();
        }
        
        private void OnEnable()
        {
            btnBack.onClickEvent.AddListener(OnBack);
        }
        
        private void OnDisable()
        {
            btnBack.onClickEvent.RemoveListener(OnBack);
        }
        
        private void OnBack()
        {
            OnSeeRateInfo?.Invoke(false);
        }
    }
}