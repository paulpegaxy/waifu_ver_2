// Author: ad   -
// Created: 08/08/2024  : : 01:08
// DateUpdate: 08/08/2024

using System;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class IdleEarnCardOverlayCanInteract : AIdleEarnCardOverlay
    {
        [SerializeField] private Image imgIconCurrency;
        [SerializeField] private TMP_Text txtPrice;
        
        protected override void OnSetData()
        {
            imgIconCurrency.sprite = ControllerSprite.Instance.GetResourceIcon(Data.typeCurrency);
            txtPrice.text = Data.cost.ToLetter();
        }
    }
}