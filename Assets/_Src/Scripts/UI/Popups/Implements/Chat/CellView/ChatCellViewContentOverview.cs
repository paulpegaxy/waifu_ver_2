using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ChatCellViewContentOverview : ESCellView<AModelChatCellView>
    {
        [SerializeField] private ItemAvatar itemAvatar;
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private TMP_Text txtMessage;
        [SerializeField] private UIButton btnInfo;
        [SerializeField] private GameObject objNotify;

        public static Action<ModelChatCellViewContentOverview> OnViewMessage;

        private ModelChatCellViewContentOverview _data;

        private void OnEnable()
        {
            btnInfo.onClickEvent.AddListener(OnInfo);
        }
        
        void OnDisable()
        {
            btnInfo.onClickEvent.RemoveListener(OnInfo);
        }

        public override void SetData(AModelChatCellView model)
        {
            _data = model as ModelChatCellViewContentOverview;
            if (_data == null) return;

            if (_data.IsGirlAvatar)
            {
                itemAvatar.SetImageAvatar(_data.GirlID);
                itemAvatar.SetOutline(_data.IsPremiumChar);
                // txtTitle.text = _data.GirlLevel+" : "+_data.GirlName;
                txtTitle.text = _data.GirlName;
            }
            else
            {
                itemAvatar.SetNameAvatar(_data.UserName);
                txtTitle.text = _data.Title;
            }

            
            var dictStorage= FactoryStorage.Get<StorageSettings>().Get().dictLvAlreadyReadMessage;
            if (dictStorage.TryGetValue(_data.GirlID, out bool isRead) && isRead)
            {
                objNotify.SetActive(false);
            }
            else
            {
                txtTitle.text = txtTitle.text.ToBold();
                txtMessage.text = txtMessage.text.ToBold();
                objNotify.SetActive(true);
            }

            txtMessage.text = _data.Message.Truncate(GameConsts.MAX_LENGHT_SHORT_MESSAGE);
        }

        private void OnInfo()
        {
            OnViewMessage?.Invoke(_data);
        }
    }
}
