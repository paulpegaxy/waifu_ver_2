using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeCharHeader : AItemCheckStartGame
{
    [SerializeField] private TMP_Text txtUserName;
    [SerializeField] private TMP_Text txtSwipeCount;
    [SerializeField] private Image imgSwipeIcon;
    [SerializeField] private Image imgAvatar;
    [SerializeField] private UIButton btnProfile;
    [SerializeField] private Color[] arrColorSwipe;

    private int _maxSwipeCount;

    private void OnEnable()
    {
        btnProfile.onClickEvent.AddListener(OnSeeProfile);
        LoadAvatar();
        ModelApiChatInfoDetail.OnChanged += OnChatInfoDetailChanged;
        if (FactoryApi.Get<ApiChatInfo>().Data.Info != null)
            OnChatInfoDetailChanged(FactoryApi.Get<ApiChatInfo>().Data.Info);
    }

    private void OnDisable()
    {
        btnProfile.onClickEvent.RemoveListener(OnSeeProfile);
        ModelApiChatInfoDetail.OnChanged -= OnChatInfoDetailChanged;
    }

    protected override void OnInit()
    {
        OnChatInfoDetailChanged(FactoryApi.Get<ApiChatInfo>().Data.Info);
    }
    
    
    private void LoadAvatar()
    {
        var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
        imgAvatar.LoadSpriteAsync("ava_" + userInfo.avatarSelected);
    }
    
    private void OnChatInfoDetailChanged(ModelApiChatInfoDetail info)
    {
        _maxSwipeCount = info.max_swipe_count;
        SetSwipeCount(info.swipe_count);
        txtUserName.text = info.extra_data.name;
    }

    private void SetSwipeCount(int count)
    {
        // txtSwipeCount.text = count + "/" + _maxSwipeCount;
        txtSwipeCount.text = count.ToString();
        bool isOutOfSwipe = count <= 0;
        imgSwipeIcon.color = isOutOfSwipe ? arrColorSwipe[1] : arrColorSwipe[0];
        txtSwipeCount.color = isOutOfSwipe ? arrColorSwipe[1] : arrColorSwipe[0];
        // txtSwipeCount.text += " Remaining\nswipe count";
    }
    
    private void OnSeeProfile()
    {
        Signal.Send(StreamId.UI.OpenUserProfile);
    }
}
