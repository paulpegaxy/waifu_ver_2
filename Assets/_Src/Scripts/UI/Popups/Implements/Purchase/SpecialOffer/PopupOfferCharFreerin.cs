using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;


namespace Game.UI
{
    public class PopupOfferCharFreerin : MonoBehaviour
    {
        [SerializeField] private ShopCellViewOfferCharFreerin information;

        private int _charId;
        
        public void SetData(ModelApiShopData data)
        {
            _charId = int.Parse(data.items[0].id);
            information.SetData(new ModelShopOfferCellView()
            {
                // Type = ShopOfferType.CharacterBundle,
                ShopItem = data
            });
        }

        private void OnEnable()
        {
            ShopCellViewOffer.OnBuySuccess += OnBuySuccess;
        }

        private void OnDisable()
        {
            ShopCellViewOffer.OnBuySuccess -= OnBuySuccess;
        }
        
        private void OnBuySuccess()
        {
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Friend_NotiAfterBuy));
            GetComponent<UIPopup>().Hide();
            LoadRewardGirl().Forget();
        }

        private async UniTask LoadRewardGirl()
        {
            var nextGirlData = DBM.Config.charPremiumConfig.GetCharData(_charId);
            var go = AnR.Get<GameObject>($"{nextGirlData.charId}_spine");
            if (go == null)
            {
                await AnR.LoadAddressableByLabels<Texture>(new List<string>() { nextGirlData.charId.ToString() });
                await AnR.LoadAddressableByLabels<GameObject>(new List<string>() { nextGirlData.charId.ToString() });
            }

            await FactoryApi.Get<ApiUpgrade>().Get();
            
            // var popup = UIPopup.Get(UIId.UIPopupName.PopupGirlPremiumReward.ToString());
            // popup.GetComponent<PopupGirlPremiumReward>().SetData(_charId,null);
            // UIPopup.AddPopupToQueue(popup);
        }
    }
}
