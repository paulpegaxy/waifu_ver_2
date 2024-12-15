using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using I2.Loc;
using Template.Defines;
using Template.Runtime;
using TMPro;
using UnityEngine;
using WebGLInput = WebGLSupport.WebGLInput;

public class DatingWindow : UIWindow
{
   [SerializeField] private Transform posSpawnEff;
   [SerializeField] private DatingHeader headerControl;
   [SerializeField] private DatingScroller scroller;
   [SerializeField] private TMP_InputField ipfChat;
   [SerializeField] private UIButton btnBack;
   [SerializeField] private UIButton btnSendChat;
   [SerializeField] private TMP_Text txtBtnSend;
   [SerializeField] private UIButton btnGotoGallery;
   [SerializeField] private Color[] arrColorText;


   private ModelApiEntityConfig _data;
   private List<ModelDatingCellView> _listCellView;
   private DataItemMessageTree _currentTree;
   private int _currentTreeIndex;
   private int _maxTreeIndex;
   private int _price;
   private bool _isFirstDating;
   private bool _isNeedRefresh;
   private string _message;
   private int _chatPoint;
   
   protected override void OnEnabled()
   {
      ModelApiChatInfoDetail.OnChanged+=OnChatInfoChanged;
      scroller.gameObject.SetActive(true);
      ipfChat.text = "";
      _isNeedRefresh = false;
      var data = this.GetEventData<TypeGameEvent, EventDataDating>(TypeGameEvent.OpenDating, true);
      Debug.LogError("DatA: " + data.entityConfig.name + ", id: " + data.entityConfig.id);
      _data = data.entityConfig;
      _isFirstDating = data.isFirstDating;

      Refresh();
      btnSendChat.onClickEvent.AddListener(OnSendChat);
      btnGotoGallery.onClickEvent.AddListener(OnGotoGallery);
      btnBack.onClickEvent.AddListener(OnBack);
      if (TelegramWebApp.IsMobile())
         WebGLInput.OnEndAndSubmit += OnEndSendChat;
      else
      {
         ipfChat.onEndEdit.AddListener(OnEndSendChat);
      }
      WebGLInput.OnHideInput += OnHideInput;
      // ipfChat.onEndEdit.AddListener(OnEndSendChat);
   }

   protected override void OnDisabled()
   {
      ModelApiChatInfoDetail.OnChanged-=OnChatInfoChanged;
      btnSendChat.onClickEvent.RemoveListener(OnSendChat);
      btnGotoGallery.onClickEvent.RemoveListener(OnGotoGallery);
      btnBack.onClickEvent.RemoveListener(OnBack);
      if (TelegramWebApp.IsMobile())
         WebGLInput.OnEndAndSubmit -= OnEndSendChat;
      else
      {
         ipfChat.onEndEdit.RemoveListener(OnEndSendChat);
      }
      WebGLInput.OnHideInput -= OnHideInput;
      // ipfChat.onEndEdit.RemoveAllListeners();
      if (_isNeedRefresh)
      {
         ProcessRefreshHistoryChat();
         this.PostEvent(TypeGameEvent.NeedRefreshChatHistory, _data.id);
      }
   }

   private void OnChatInfoChanged(ModelApiChatInfoDetail data)
   {
      _chatPoint = data.chat_point;
      _price = data.price_send_chat;
      txtBtnSend.text = $"-{_price}";
      bool isEnoughPoint = ControllerResource.IsEnough(TypeResource.ChatPoint, _price);
      txtBtnSend.color = isEnoughPoint ? arrColorText[0] : arrColorText[1];
   }

   private async void ProcessRefreshHistoryChat()
   {
      await FactoryApi.Get<ApiChatAI>().GetChatHistory(_data.id);
   }

   private async void Refresh()
   {
      _listCellView = new List<ModelDatingCellView>();
      var apiChatInfo = FactoryApi.Get<ApiChatInfo>().Data.Info;
      _price = apiChatInfo.price_send_chat;
      _chatPoint = apiChatInfo.chat_point;
      txtBtnSend.text = $"-{_price}";
      bool isEnoughPoint = ControllerResource.IsEnough(TypeResource.ChatPoint, _price);
      txtBtnSend.color = isEnoughPoint ? arrColorText[0] : arrColorText[1];
      headerControl.SetData(_data);

      List<ModelApiChatHistory> listHistoryChat = new();
      if (!_isFirstDating || _data.exp > 0)
      {
         listHistoryChat = FactoryApi.Get<ApiChatAI>().Data.GetChatHistory(_data.id);
      }

      if (listHistoryChat?.Count > 0)
      {
         for (var i = 0; i < listHistoryChat.Count; i++)
         {
            var ele = listHistoryChat[i];
            await LoadCellViewChat(ele.message, ele.sender_type == TypeSenderChatHistory.character,
               ele.GetPictureMessage());
         }
      }
       
      ReloadScroller();
   }

   private async UniTask LoadChat(string message,bool isWaifu,string pictureMessage="")
   {
      await LoadCellViewChat(message, isWaifu, pictureMessage);
      ReloadScroller();
   }

   private async UniTask LoadCellViewChat(string message,bool isWaifu,string pictureMessage="")
   {
      if (isWaifu)
      {
         _listCellView.Add(new ModelDatingCellViewContentOtherMessage()
         {
            Message = message,
            Config = _data,
            SprAvatar = headerControl.GetSpriteImage()
         });
      }
      else
      {
         _listCellView.Add(new ModelDatingCellViewContentMyMessage()
         {
            Message = message
         });
      }
      
      if (!string.IsNullOrEmpty(pictureMessage))
      {
         await UniTask.Delay(500);
         _listCellView.Add(new ModelDatingCellViewContentPicture()
         {
            EntityConfig = _data,
            PictureMessage = pictureMessage,
            SprAva = headerControl.GetSpriteImage()
         });
      }
   }

   private void LoadCellViewPicture()
   {
      
   }
   
   private void LoadCellViewTyping(bool isAdditive)
   {
      if (isAdditive)
      {
         _listCellView.Add(new ModelDatingCellViewContentTyping()
         {
            SprAvatar = headerControl.GetSpriteImage()
         });
      }
      else
      {
         _listCellView.RemoveAll(x => x.Type == DatingCellViewType.ContentTyping);
      }
      ReloadScroller();
   }
   
   private void OnEndSendChat(string message)
   {
      // ControllerPopup.ShowToast("OnEndSendChat");
      _message = message;
      ipfChat.text = "";
      ProcessOnSendChat(_message);
   }
   
   private void OnEndSendChat()
   {
      // ControllerPopup.ShowToast("OnEndSendChat");
      _message = ipfChat.text;
      ProcessOnSendChat(_message);
   }
   
   private void OnHideInput()
   {
      // ControllerPopup.ShowToast("OnHideInput");
      ipfChat.text = "";
   }

   private void OnSendChat()
   {
      ProcessOnSendChat(ipfChat.text);
      ipfChat.text = "";
   }

   private async void ProcessOnSendChat(string message)
   {
      if (string.IsNullOrEmpty(message))
      {
         ControllerPopup.ShowToastError("Please input message");
  
         return;
      }

      if (_chatPoint < _price)
      {
         ControllerPopup.ShowInformation("Not enough chat point");
         return;
      }

      _chatPoint -= _price;
      string userMess = message.Trim();
      // ipfChat.text = "";
      await LoadChat(userMess, false);
      // await UniTask.Delay(250);
      LoadCellViewTyping(true);
      try
      {
         var data = await FactoryApi.Get<ApiChatAI>().PostChatAI(_data.id, userMess);
         await FactoryApi.Get<ApiChatInfo>().GetInfo();
         LoadCellViewTyping(false);
         string pictureMessage = data.reply.GetPictureMessage();
         await LoadChat(data.reply.message, true, pictureMessage);
         SpawnEffect(pictureMessage);
         _isNeedRefresh = true;
      }
      catch (Exception e)
      {
         e.ShowError();
      }
   }

   private void SpawnEffect(string pictureMessage)
   {
      if (!string.IsNullOrEmpty(pictureMessage))
      {
         ControllerUI.Instance.Spawn(TypeResource.ExpWaifu, posSpawnEff.position, 20,2f);
      }
      else
      {
         ControllerUI.Instance.Spawn(TypeResource.ExpWaifu, posSpawnEff.position, 1,2f);
      }
   }

   private void ReloadScroller()
   {
      scroller.SetData(_listCellView);
      scroller.JumpToDataIndex(_listCellView.Count - 1);
   }

   private void OnGotoGallery()
   {
      if (_data == null)
      {
         Debug.LogError("Data is null");
         return;
      }
      
      this.GotoWaifuProfileWindow(_data);
   }
   
   private void OnBack()
   {
      scroller.gameObject.SetActive(false);
      _listCellView = new List<ModelDatingCellView>();
      scroller.SetData(_listCellView);
   }
}

[Serializable]
public class EventDataDating
{
   public bool isFirstDating;
   public ModelApiEntityConfig entityConfig;
}