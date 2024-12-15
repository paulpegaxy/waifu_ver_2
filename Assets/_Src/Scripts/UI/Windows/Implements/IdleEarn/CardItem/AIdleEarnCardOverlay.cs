// Author: ad   -
// Created: 08/08/2024  : : 01:08
// DateUpdate: 08/08/2024

using Doozy.Runtime.UIManager.Components;
using Slime.UI;
using UnityEngine;

namespace Game.UI
{
    public abstract class AIdleEarnCardOverlay : BaseCardItem<DataIdleEarnUpgradeItem>
    {
        [SerializeField] private UIButton btnUpgrade;
        
        private void OnEnable()
        {
            btnUpgrade?.onClickEvent.AddListener(OnClick);   
        }

        private void OnDisable()
        {
            btnUpgrade?.onClickEvent.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            var popup = this.ShowPopup<PopupConfirmIdleEarnUpgrade>(UIId.UIPopupName.PopupConfirmIdleEarnUpgrade);
            popup.InitShow(Data);
        }
    }
}