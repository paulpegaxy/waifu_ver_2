using System.Collections;
using System.Collections.Generic;
using Game.Model;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupConfrmPurchasePrice : PopupConfirmPurchase
{
    [SerializeField] private Image imgReceiveCurrency;
    [SerializeField] private TMP_Text txtReceiveValue;

    public void SetValueReceived(ModelResource resourcePrice)
    {
        txtReceiveValue.text = resourcePrice.Amount.ToLetter();
        imgReceiveCurrency.sprite = ControllerSprite.Instance.GetResourceIcon(resourcePrice.Type);
    }
}
