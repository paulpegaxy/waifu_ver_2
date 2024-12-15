using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using Game.UI;
using UnityEngine;

public class FooterBarControl : AItemCheckStartGame
{
    [SerializeField] private GameObject objHolder;
    [SerializeField] private UIToggleGroup tggFooter;
    
    private List<UIToggle> _toggles;
    
    private SignalStream _signalStream;
    private SignalStream _signalStreamButton;
    
    private SignalReceiver _signalReceiver;
    
    
    private int _index = -1;

    protected override void Awake()
    {
        base.Awake();
        objHolder.SetActive(false);
        UIWindow.OnDataLoaded += OnDataLoaded;
    }

    private void Start()
    {
        _signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
        _signalStream = SignalStream.Get(nameof(UIContainer), nameof(UIView)).ConnectReceiver(_signalReceiver);
        _signalStreamButton = SignalStream.Get(nameof(UISelectable), nameof(UIButton)).ConnectReceiver(_signalReceiver);
        
        tggFooter.OnToggleTriggeredCallback.AddListener(OnTriggerToggle);
        _toggles = new List<UIToggle>();
        for (int i = 0; i < tggFooter.transform.childCount; i++)
        {
            _toggles.Add(tggFooter.transform.GetChild(i).GetComponent<UIToggle>());
        }
    }

    protected override void OnDestroy()
    {
        tggFooter.OnToggleTriggeredCallback.RemoveListener(OnTriggerToggle);
        _signalStream.DisconnectReceiver(_signalReceiver);
        _signalReceiver = null;
        UIWindow.OnDataLoaded -= OnDataLoaded;
        base.OnDestroy();
    }
    
    protected override void OnInit()
    {
        // objHolder.SetActive(true);
        FactoryApi.Get<ApiChatInfo>().Data.Info.Notification();
    }
    
    private void OnTriggerToggle(UIToggle toggle)
    {
        var lastIndex = tggFooter.lastToggleOnIndex;
        if (_index == lastIndex)
        {
            return;
        }
        
        _index = lastIndex;
        switch (lastIndex)
        {
            case 0:
                Signal.Send(StreamId.UI.OpenShop);
                break;
            // case 1:
            //     Signal.Send(StreamId.UI.OpenQuest);
            //     break;
            case 1:
                Signal.Send(StreamId.UI.BackToSwipe);
                break;
            // case 3:
            //     Signal.Send(StreamId.UI.OpenFriend);
            //     break;
            case 2:
                Signal.Send(StreamId.UI.ChatList);
                break;
        }
    }
    
    private void OnDataLoaded(UIId.UIViewCategory category, UIId.UIViewName name)
    {
        CheckType(name);
    }

    private void OnSignal(Signal signal)
    {
        if (signal.valueAsObject is UIViewSignalData)
        {
            UIViewSignalData data = (UIViewSignalData) signal.valueAsObject;
            var type = data.viewName.ToEnum<UIId.UIViewName>();
            CheckType(type);
        }
        else if (signal.valueAsObject is UIButtonSignalData)
        {
            var data = (UIButtonSignalData)signal.valueAsObject;
            // Debug.LogError("data: " + data.buttonName + " " + data.buttonCategory);
            CheckTypeButton(data);
        }
    }
    
    private void CheckTypeButton(UIButtonSignalData data)
    {
        switch (data.buttonName)
        {
            case "BackToChatList":
                if (!_toggles[^1].isOn)
                    _toggles[^1].SetIsOn(true);
                break;
            case "BackFromWaifuProfile":
                if (!_toggles[1].isOn)
                    _toggles[1].SetIsOn(true);
                break;
        }
    }

    private void CheckType(UIId.UIViewName type)
    {
        switch (type)
        {
            case UIId.UIViewName.ChatList:
            case UIId.UIViewName.SwipeChar:
            case UIId.UIViewName.Shop:
                objHolder.SetActive(true);
                break;
            default:
                objHolder.SetActive(false);
                break;
        }
    }
}
