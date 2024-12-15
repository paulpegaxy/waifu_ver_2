
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;

public class ChatPanelDetail : AChatPanel
{
    [SerializeField] private ItemAvatar itemAvatar;
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtMessage;
    // [SerializeField] private ChatScroller scroller;

    [SerializeField] private Transform posContainMessage;
    
    [SerializeField] private UIButton btnBack;
    [SerializeField] private UIButton btnBuyChatPremium;
    [SerializeField] private UIButton btnSend;
    [SerializeField] private TMP_InputField ipfMessage;

    public Action OnBack;

    private int _girlId;
    
    private void OnEnable()
    {
        btnBack.onClickEvent.AddListener(OnClickBack);
        btnBuyChatPremium.onClickEvent.AddListener(OnBuyChatPremium);
        btnSend.onClickEvent.AddListener(OnSendMessage);
    }
    
    void OnDisable()
    {
        btnBack.onClickEvent.RemoveListener(OnClickBack);
        btnBuyChatPremium.onClickEvent.RemoveListener(OnBuyChatPremium);
        btnSend.onClickEvent.RemoveListener(OnSendMessage);
    }

    public void Show(ModelChatCellViewContentOverview data)
    {
        _girlId = data.GirlID;
        txtTitle.text = data.GirlName;
        string message = "";
        txtMessage.text = $"{data.GirlName} inbox";
        var listData = new List<ModelChatCellViewContentOtherMessage>();
        if (data.IsGirlAvatar)
        {
            itemAvatar.SetImageAvatar(data.GirlID);
            itemAvatar.SetOutline(data.IsPremiumChar);

            if (!data.IsPremiumChar)
            {
                var dataRank = DBM.Config.rankingConfig.GetRankDataBasedGirlId(data.GirlID);
                if (dataRank != null)
                {
                    int reachLevelCount = data.ReachAtLevel;
                    for (int i = 0; i < reachLevelCount; i++)
                    {
                        message = ExtensionEnum.ToMessage(dataRank.girlId, i);
                        listData.Add(new ModelChatCellViewContentOtherMessage()
                        {
                            GirlID = data.GirlID,
                            Message = message,
                        });
                    }
                }
                else
                {
                    GameUtils.Log("red", "null data rank at girl id: " + data.GirlID);
                }
            }
            else
            {
                for (int i = 1; i <= data.ReachAtLevel; i++)
                {
                    message = ExtensionEnum.ToMessageCharPremium(data.GirlID, i);
                    listData.Add(new ModelChatCellViewContentOtherMessage()
                    {
                        GirlID = data.GirlID,
                        Message = message,
                        IsPremium = true,
                    });
                }
            }
        }

        posContainMessage.FillData<ModelChatCellViewContentOtherMessage, ChatCellViewContentOtherMessage>
        (listData, (dataItem, view, index) =>
        {
            view.SetData(dataItem);
        });

        // scroller.SetData(listData);
        SaveLocalMessage();
        Fetch();
    }

    private void SaveLocalMessage()
    {
        var storageSetting = FactoryStorage.Get<StorageSettings>();
        var apiGameInfo = FactoryApi.Get<ApiGame>().Data.Info;
        var model = storageSetting.Get();
        if (model.dictLvAlreadyReadMessage.TryGetValue(_girlId, out bool isRead))
        {
            if (!isRead)
            {
                model.dictLvAlreadyReadMessage[_girlId] = true;
            }
        }else
        {
            model.dictLvAlreadyReadMessage.Add(_girlId, true);
        }

        storageSetting.Save();
        apiGameInfo.Notification();
    }
    
    
    private void OnClickBack()
    {
        gameObject.SetActive(false);
        OnBack?.Invoke();
    }
    
    private void OnBuyChatPremium()
    {
        // Show popup buy chat premium
        ControllerPopup.ShowToastComingSoon();
    }
    
    private void OnSendMessage()
    {
        // Show popup send message
        ControllerPopup.ShowToastComingSoon();
    }

    protected override async UniTask OnFetchData()
    {
        await UniTask.DelayFrame(2);
    }
}
