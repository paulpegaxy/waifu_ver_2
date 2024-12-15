// Author: ad   -
// Created: 17/10/2024  : : 06:10
// DateUpdate: 17/10/2024

using System;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace Game.UI
{
    public abstract class AItemButtonGirlCardOverlayPremium : AItemButtonGirlCardOverlay
    {
        [SerializeField] protected UIButton btnClick;

        protected virtual void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
        }
        
        protected virtual void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
        }
        
        protected abstract void OnClick();
    }
}