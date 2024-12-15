using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PopupGirlPremiumReward : APopupGirlReward
    {
        [SerializeField] private UIButton btnGoto;

        protected override void OnEnable()
        {
            base.OnEnable();
            btnGoto.onClickEvent.AddListener(OnClickGoto);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            btnGoto.onClickEvent.RemoveListener(OnClickGoto);
        }

        public void SetData(int charID,Action initDone)
        {
            // var data = FactoryApi.Get<ApiUpgrade>().Dat.
            imgCharName.DOFade(0, 0);
            var charData = DBM.Config.charPremiumConfig.GetCharData(charID);
            txtCharName.text = charData.name;
            txtDes.text = ExtensionEnum.ToRewardGirlMessage(charID);
            itemAvatar.SetImageAvatar(charID);
            txtTotalBonus.text = "";
            
            _entity = ControllerSpawner.Instance.SpawnGirl(charID, posHolderChar);
            _entity.InitToShowReward(charID, () =>
            {
                // objEffectLight.SetActive(false);
                var charData = FactoryApi.Get<ApiUpgrade>().Data.GetPremiumChar(charID);

                txtTotalBonus.text = $"+{charData.PointProfitParse.ToLetter()} SUGAR/h";
            
                imgCharName.LoadSpriteAutoParseAsync("name_" + charID);
                initDone?.Invoke();
            });
            // objEffectLight.SetActive(true);
            anim.Play();
        }

        private void OnClickGoto()
        {
            GetComponent<UIPopup>().Hide();
            this.PostEvent(TypeGameEvent.OpenPremiumGallery, true);
            Signal.Send(StreamId.UI.OpenGallery);
        }
        
        protected override void OnSetData()
        {
            
        }
    }
}