using System;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class RecentChatCellViewContent : ESCellView<ModelRecentChatCellView>
	{
		[SerializeField] private UIButton btnGotoChat;
		[SerializeField] private ItemWaifuAvatar itemWaifuAvatar;
		[SerializeField] private TMP_Text txtName;
		[SerializeField] private TMP_Text txtMessage;

		private ModelApiEntityConfig _data;
		
		public override void SetData(ModelRecentChatCellView model)
		{
			if (model is ModelRecentChatCellViewContent data)
			{
				if (data.Data.sender_type == TypeSenderChatHistory.user)
				{
					UnityEngine.Debug.Log("sender is user");
					return;
				}
				
				var entityConfig = FactoryApi.Get<ApiEntity>().Data.GetEntity(data.Data.sender);
				_data = entityConfig;
				txtName.text = entityConfig.name;
				txtMessage.text = data.Data.ShortMessage();
				itemWaifuAvatar.SetAvatar(entityConfig);
				// UnityEngine.Debug.Log("ava key: " + entityConfig.AvaCharKey);
				// imgAvatar.LoadSpriteAsync(entityConfig.AvaCharKey);
			}
		}

		private void OnEnable()
		{
			btnGotoChat.onClickEvent.AddListener(OnGotoChat);
		}

		private void OnDisable()
		{
			btnGotoChat.onClickEvent.RemoveListener(OnGotoChat);
		}

		private void OnGotoChat()
		{
			this.GotoDatingWindow(_data);
		}
	}
}