using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Template.Defines;
using UnityEngine;

public class PopupOfferEventBundle : MonoBehaviour
{
    [SerializeField] private UIButton btnGoto;

    private string _eventId;

    public void SetData(string eventId)
    {
        _eventId = eventId;
    }

    private void OnEnable()
    {
        btnGoto.onClickEvent.AddListener(OnGoto);
    }

    private void OnDisable()
    {
        btnGoto.onClickEvent.RemoveListener(OnGoto);
    }
    
    private void OnGoto()
    {
        GetComponent<UIPopup>().Hide();
        this.PostEvent(TypeGameEvent.OpenEventBundle, _eventId);
        Signal.Send(StreamId.UI.OpenEventBundle);
    }
}
