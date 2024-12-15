using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;

public class ChatPanelOverView : AChatPanel
{
    [SerializeField] private ChatScroller scroller;

    private List<AModelChatCellView> _listData;
    
    protected override async UniTask OnFetchData()
    {
        _listData = new List<AModelChatCellView>();
        var dataList = SpecialExtensionGame.GetListContentChat();
        dataList = dataList.OrderByDescending(x => x.GirlID).ToList();
        for (int i = 0; i < dataList.Count; i++)
        {
            _listData.Add(dataList[i]);
        }
        // GetCurrentGirlMessage();
        scroller.SetData(_listData);
        scroller.JumpToDataIndex(_listData.Count - 1);
    }
}
