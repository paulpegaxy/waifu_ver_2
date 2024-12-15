using System;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemAvatar : AItemCheckStartGame
    {
        [SerializeField] private GameObject holderAva;
        [SerializeField] private GameObject holderName;
        [SerializeField] private Image imageAvatar;
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private bool isUserImageAvatar;
        [SerializeField] private bool isManualLoad;
        [SerializeField] private GameObject[] arrObjOutline;

        protected override void Awake()
        {
            base.Awake();
            holderAva.SetActive(true);
            holderName.SetActive(false);
        }

        protected override void OnEnabled()
        {
            this.RegisterEvent(TypeGameEvent.NextGirlSuccess, OnNextGirl);
        }

        protected override void OnDisabled()
        {
            this.RemoveEvent(TypeGameEvent.NextGirlSuccess, OnNextGirl);
        }

        private void OnNextGirl(object data)
        {
            Refresh();
        }

        protected override void OnInit()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (isUserImageAvatar && !isManualLoad)
            {
                var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
                var userInfo = storageUserInfo.Get();
                // if (FactoryApi.Get<ApiGame>().Data.Info != null)
                SetImageAvatar(userInfo.selectedWaifuId);
            }
        }

        public void SetImageAvatar(int girlId)
        {
            // UnityEngine.Debug.LogError("GIRL ID: "+girlId);
            holderAva.SetActive(true);
            holderName.SetActive(false);
            imageAvatar.LoadSpriteAutoParseAsync("ava_" + girlId);
            // imageAvatar.sprite = ControllerSprite.Instance.GetGirlAvatar(girlId);

        }

        public void SetOutline(bool isPremium)
        {
            arrObjOutline[0].SetActive(!isPremium);
            arrObjOutline[1].SetActive(isPremium);
        }


        public void SetNameAvatar(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                // holderAva.SetActive(true);
                // holderName.SetActive(false);
                // return;
                name = "-";
            }
            var representativeName = name.Substring(0, 1).ToUpperCase();

            holderAva.SetActive(false);
            holderName.SetActive(true);
            txtName.text = representativeName;
        }
    }
}