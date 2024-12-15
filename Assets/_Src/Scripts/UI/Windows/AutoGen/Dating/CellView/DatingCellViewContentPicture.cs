using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class DatingCellViewContentPicture :  ESCellView<ModelDatingCellView>
    {
        [SerializeField] private Image imgAva;
        [SerializeField] private ItemWaifuAvatar itemWaifuAvatar;
        [SerializeField] private UIButton btnAva;
        
        public override void SetData(ModelDatingCellView model)
        {
            if (model is ModelDatingCellViewContentPicture data)
            {
                // string pic = data.PictureMessage;
                // string var = "40001_picturelv_2";
                
                itemWaifuAvatar.SetAvatar(data.SprAva, data.EntityConfig);
                var split = data.PictureMessage.Split('_');
                var lv= split[^1].Replace("lv", "");
                var lvParse = int.Parse(lv);
                imgAva.LoadSpriteAsync(data.EntityConfig.GetMediaPictureKey(lvParse));
            }
        }

        private void OnEnable()
        {
            btnAva.onClickEvent.AddListener(OnClickAva);
        }
        
        private void OnDisable()
        {
            btnAva.onClickEvent.RemoveListener(OnClickAva);
        }
        
        private void OnClickAva()
        {
            // Debug.Log("OnClickAva");
            var popup = this.ShowPopup<PopupSeeWaifuPicture>(UIId.UIPopupName.PopupSeeWaifuPicture);
            popup.SetData(imgAva.sprite);
        }
    }
}