using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;
using UnityEngine;

public class ItemTotalUser : MonoBehaviour
{
    [SerializeField] private TMP_Text txtTotalUser;
    [SerializeField] private UIButton btnClick;

    private void Awake()
    {
        btnClick.onClickEvent.AddListener(OnClickButton);
    }

    private void OnDestroy()
    {   
        btnClick.onClickEvent.RemoveListener(OnClickButton);
    }

    private async void OnEnable()
    {
        var apiCommon = FactoryApi.Get<ApiCommon>();
        if (apiCommon.Data.data == null)
            return;
        
        await apiCommon.GetSummary();
        if (apiCommon.Data.data != null)
            txtTotalUser.text =
                $"{apiCommon.Data.data.total_user:#,##0} {Localization.Get(TextId.Common_LbWaifu)}";
    }
    
    private void OnClickButton()
    {
        Signal.Send(StreamId.UI.OpenPlayerSummary);
    }
}
