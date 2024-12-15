using System;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class AGalleryItem<T> : MonoBehaviour where T : DataItemGallery
    {
        [SerializeField] protected TMP_Text txtName;
        [SerializeField] protected Image imgItem;
        [SerializeField] protected GameObject objLocked;
        [SerializeField] protected GameObject objCompleted;
        [SerializeField] protected UIButton btnClick;

        private void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
        }
        private void OnDisable()

        {
            btnClick.onClickEvent.RemoveListener(OnClick);
        }

        public void SetData(T data)
        {
            gameObject.SetActive(false);
            var dataParse = data as DataItemGallery;
            txtName.text = dataParse.name;
            btnClick.interactable = data.isUnlock;
            objLocked.SetActive(!data.isUnlock);
            if (!data.isUnlock)
                txtName.text = "Not Unlocked";

            if (imgItem != null)
                imgItem.DOFade(0, 0);
            
            if (!string.IsNullOrEmpty(dataParse.avatar_name))
                imgItem.LoadSpriteAutoParseAsync(dataParse.avatar_name);
            OnSetData(data);
            gameObject.SetActive(true);
        }

        protected abstract void OnSetData(T data);

        protected virtual void OnClick()
        {

        }
    }

    [Serializable]
    public class DataItemGallery
    {
        public string name;
        public string avatar_name;
        public bool isUnlock;
        public int index;
        public int girlId;
    }
}