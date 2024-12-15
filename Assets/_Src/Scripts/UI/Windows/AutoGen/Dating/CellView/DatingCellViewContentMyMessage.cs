using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class DatingCellViewContentMyMessage : ESCellView<ModelDatingCellView>
    {
        [SerializeField] private Image imgAvatar;
        [SerializeField] private TMP_Text txtMessage;
        
        public override void SetData(ModelDatingCellView model)
        {
            if (model is ModelDatingCellViewContentMyMessage data)
            {
                var kvp = data.Message.InsertLineBreaksByCharacters(GameConsts.MAX_COUNT_CHAR_CHAT);
                txtMessage.text = kvp.Value;
                var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
                imgAvatar.LoadSpriteAsync("ava_" + userInfo.avatarSelected);
            }
        }
    }
}