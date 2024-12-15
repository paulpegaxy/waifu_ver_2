using Game.Model;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ChatCellViewContentOtherMessage : ESCellView<AModelChatCellView>
    {
        [SerializeField] private ItemAvatar itemAvatar;
        [SerializeField] private TMP_Text txtMessage;
        [SerializeField] private GameObject objNotify;
        
        public override void SetData(AModelChatCellView model)
        {
            var data = model as ModelChatCellViewContentOtherMessage;
            if (data != null)
            {
                itemAvatar.SetImageAvatar(data.GirlID);
                itemAvatar.SetOutline(data.IsPremium);
                // txtMessage.text = data.Message.InsertLineBreaks(GameConsts.MAX_LENGHT_PER_LINE_MESSAGE);
                txtMessage.text = data.Message.InsertLineBreaksAfterWords(GameConsts.MAX_WORD_PER_LINE);
            }

            // txtMessage.text = data.Message.InsertLineBreaks(GameConsts.MAX_LENGHT_PER_LINE_MESSAGE);
        }
    }
}