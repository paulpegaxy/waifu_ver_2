using System;
using System.Collections;
using System.Collections.Generic;
using Game.Extensions;
using Game.Model;
using Game.UI;
using Template.Defines;
using UnityEngine;

public class PopupChat : MonoBehaviour
{
    [SerializeField] private ChatPanelOverView panelOverview;
    [SerializeField] private ChatPanelDetail panelDetailMessage;

    public void Show(int girlId = 0)
    {
        if (girlId != 0)
        {
            var item = SpecialExtensionGame.GetListContentChat().Find(x => x.GirlID == girlId);
            if (item != null)
                OnViewMessage(item);
            else
                OnBackFromDetail();
        }
        else
        {
            TurnOnChatDetail(false);
            panelOverview.Fetch();
        }

        ChatCellViewContentOverview.OnViewMessage += OnViewMessage;
        panelDetailMessage.OnBack += OnBackFromDetail;
    }

    private void OnDisable()
    {
        ChatCellViewContentOverview.OnViewMessage -= OnViewMessage;
        panelDetailMessage.OnBack -= OnBackFromDetail;
    }
    
    private void OnViewMessage(ModelChatCellViewContentOverview data)
    {
        TurnOnChatDetail(true);
        panelDetailMessage.Show(data);   
    }

    private void OnBackFromDetail()
    {
        TurnOnChatDetail(false);
        panelOverview.Fetch();
    }

    private void TurnOnChatDetail(bool isOn)
    {
        panelOverview.gameObject.SetActive(!isOn);
        panelDetailMessage.gameObject.SetActive(isOn);
    }
}

[Serializable]
public class DataEventChatMessage
{
    public int girlId;
}