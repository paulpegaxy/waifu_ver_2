using BreakInfinity;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Slime.UI;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public abstract class AConfirmIdleEarnPanelUnlock : AConfirmIdleEarnPanel
    {
        [SerializeField] protected TMP_Text txtDes;
        [SerializeField] protected TMP_Text txtPrice;
        [SerializeField] protected UIButton btnUnlock;
        
        protected virtual void OnEnable()
        {
            btnUnlock.onClickEvent.AddListener(OnClickUnlock);
        }

        protected virtual void OnDisable()
        {
            btnUnlock.onClickEvent.RemoveListener(OnClickUnlock);
        }

        private void OnClickUnlock()
        {
            OnUnlock();
        }

        protected abstract void OnUnlock();
    }
}