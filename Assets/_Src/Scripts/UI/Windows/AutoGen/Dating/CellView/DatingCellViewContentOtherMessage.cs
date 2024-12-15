using System;
using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class DatingCellViewContentOtherMessage : ESCellView<ModelDatingCellView>
	{
		[SerializeField] private ItemWaifuAvatar itemWaifuAvatar;
		[SerializeField] private TMP_Text txtMessage;

		public override void SetData(ModelDatingCellView model)
		{
			if (model is ModelDatingCellViewContentOtherMessage data)
			{
				itemWaifuAvatar.SetAvatar(data.SprAvatar, data.Config);
				var kvp = data.Message.InsertLineBreaksByCharacters(GameConsts.MAX_COUNT_CHAR_CHAT);
				txtMessage.text = kvp.Value;
			}
		}
	}
}