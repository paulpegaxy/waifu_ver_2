// Author: ad   -
// Created: 02/12/2024  : : 02:12
// DateUpdate: 02/12/2024

using Doozy.Runtime.UIManager.Components;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class WaifuProfileItemPicture : MonoBehaviour
    {
        [SerializeField] private Image imgAva;
        [SerializeField] private GameObject objLocked;
        [SerializeField] private UIButton btnAva;

        public void SetData(DataItemWaifuProfilePicture data)
        {
            var index = data.index;
            // UnityEngine.Debug.LogError("index " + index + " level " + data.data.level + ". id: " + data.data.id);
            var indexCalculate = index + 1;
            bool isLocked = indexCalculate > data.data.level;
            objLocked.SetActive(isLocked);
            imgAva.LoadSpriteAsync(data.data.GetMediaPictureKey(indexCalculate));
            btnAva.interactable = !isLocked;
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