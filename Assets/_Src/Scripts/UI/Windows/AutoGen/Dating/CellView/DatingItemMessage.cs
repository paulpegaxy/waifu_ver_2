// Author: ad   -
// Created: 13/11/2024  : : 10:11
// DateUpdate: 13/11/2024

using Game.Model;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DatingItemMessage : MonoBehaviour
    {
        [SerializeField] private GameObject[] arrObjSpeaker;
        [SerializeField] private ItemAvatar itemAvatar;
        [SerializeField] private TMP_Text txtMessage;
		
        public void SetData(ModelDatingCellView model)
        {
            if (model is ModelDatingCellViewContentOtherMessage dataOther)
            {
                SetSpeaker(false);
                // itemAvatar.SetImageAvatar(dataOther.GirlID);
                itemAvatar.SetOutline(false);
                txtMessage.text = dataOther.Message.InsertLineBreaksAfterWords(GameConsts.MAX_WORD_PER_LINE);
            }else if (model is ModelDatingCellViewContentMyMessage dataMy)
            {
                SetSpeaker(true);
            }
        }

        private void SetSpeaker(bool isMyMessage)
        {
            arrObjSpeaker[0].SetActive(!isMyMessage);
            arrObjSpeaker[1].SetActive(isMyMessage);
        }
    }
}