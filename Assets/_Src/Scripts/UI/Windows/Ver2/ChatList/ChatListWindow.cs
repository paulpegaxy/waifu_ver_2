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
using TMPro;
using UnityEngine;

public class ChatListWindow : UIWindow
{
   [SerializeField] private TMP_InputField ipfSearch;
   [SerializeField] private GameObject objHolderListChar;
   [SerializeField] private ListCharScroller scrollerListChar;
   [SerializeField] private RecentChatScroller scrollerRecentChat;
   [SerializeField] private GameObject[] arrObjContent;

   private Dictionary<int, List<ModelApiChatHistory>> _dictCharChat;

   protected override void OnEnabled()
   {
      base.OnEnabled();
      Refresh();
      ipfSearch.onValueChanged.AddListener(OnSearch);
   }

   protected override void OnDisabled()
   {
      base.OnDisabled();
      ipfSearch.onValueChanged.RemoveListener(OnSearch);
   }

   private async void Refresh()
   {
      var apiEntity = FactoryApi.Get<ApiEntity>();
      await apiEntity.Get();
      var listChar = apiEntity.Data.MatchedChars;
      ipfSearch.text = "";
      TurnOnContentEmpty(listChar.Count <= 0);
      objHolderListChar.SetActive(listChar?.Count > 0);
      LoadAllCharacterMatched();
      int charIdForceSync = -1;
      var id=this.GetEventData<TypeGameEvent, int>(TypeGameEvent.NeedRefreshChatHistory, true);
      if (id != 0)
      {
         charIdForceSync = id;
      }
      
      this.ShowProcessing();
      // try
      // {
         await ReloadCharacterChatData(listChar, charIdForceSync);
         LoadRecentChatScroller();
         this.HideProcessing();
      // }
      // catch (Exception e)
      // {
      //   e.ShowError();
      // }
   }
   
   private void OnSearch(string value)
   {
      if (string.IsNullOrEmpty(value))
      {
         LoadAllCharacterMatched();
         return;
      }
      var listChar = FactoryApi.Get<ApiEntity>().Data.MatchedChars;
      var listCharFiltered = listChar.Where(x => x.name.ToLower().Contains(value.ToLower())).ToList();
      scrollerListChar.SetData(listCharFiltered);
   }

   private void LoadAllCharacterMatched()
   {
      var apiEntity = FactoryApi.Get<ApiEntity>();
      var listChar = apiEntity.Data.MatchedChars;
      scrollerListChar.SetData(listChar);
   }

   private async UniTask ReloadCharacterChatData(List<ModelApiEntityConfig> listData,int charIdForceSync=-1)
   {
      _dictCharChat = new();
      var listId = listData.Select(x => x.id).ToList();
      _dictCharChat = await FactoryApi.Get<ApiChatAI>().FetchAllChatHistory(listId, charIdForceSync);
   }

   private void LoadRecentChatScroller()
   {
      List<ModelRecentChatCellView> listRecentData = new List<ModelRecentChatCellView>();
      foreach (var item in _dictCharChat)
      {
         if (item.Value.Count > 0)
         {
            var list = item.Value.OrderBy(x => x.id).ToList();
            var lastEle = list.LastOrDefault(x => x.sender_type != TypeSenderChatHistory.user);
            listRecentData.Add(new ModelRecentChatCellViewContent()
            {
               Data = lastEle
            });
         }
      }
      
      scrollerRecentChat.SetData(listRecentData);
   }

   private void TurnOnContentEmpty(bool isEmptyContent)
   {
      arrObjContent[0].SetActive(!isEmptyContent);
      arrObjContent[1].SetActive(isEmptyContent);
   }
}
