using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

public class PopupSuccessMatch : MonoBehaviour
{
    [SerializeField] private UIButton btnChatNow;
    [SerializeField] private Image imgBanner;

    private ModelApiEntityConfig _data;

    public void SetData(ModelApiEntityConfig data)
    {
        _data = data;
        imgBanner.LoadSpriteAsync(data.BgCharKey);
    }

    private void OnEnable()
    {
        btnChatNow.onClickEvent.AddListener(OnChatNow);
    }

    private void OnDisable()
    {
        btnChatNow.onClickEvent.RemoveListener(OnChatNow);
    }

    private void OnChatNow()
    {
        GetComponent<UIPopup>().Hide();
        this.GotoDatingWindow(_data,true);
    }
}
