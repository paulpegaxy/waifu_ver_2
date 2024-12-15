using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.UI;
using UnityEngine;

public class UserProfileWindow : UIWindow
{
    [SerializeField] private UIButton btnBack;
    
    protected override void OnEnabled()
    {
        base.OnEnabled();
        btnBack.onClickEvent.AddListener(OnClickBack);
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        btnBack.onClickEvent.RemoveListener(OnClickBack);
    }
        
    private void OnClickBack()
    {
        Signal.Send(StreamId.UI.BackToSwipe);
    }
}
