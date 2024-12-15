
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Extensions;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Game.UI
{
    public class PopupGirlReward : APopupGirlReward
    {
        protected override void OnSetData()
        {
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            if (gameInfo == null)
            {
                GameUtils.Log("red", "PopupGirlReward: gameInfo is null");
                return;
            }


            txtCharName.text = gameInfo.DataCurrentGirlSelected.girlName;
            int girlId = gameInfo.DataCurrentGirlSelected.girlId;

            imgCharName.DOFade(0, 0);

            txtDes.text = ExtensionEnum.ToRewardGirlMessage(girlId);
            itemAvatar.SetImageAvatar(girlId);
            txtTotalBonus.text = "";

            _entity = ControllerSpawner.Instance.SpawnGirl(girlId, posHolderChar);
            _entity.InitToShowReward(girlId, () =>
            {
                // objEffectLight.SetActive(false);
                txtTotalBonus.text = $"+{gameInfo.next_girl_level_data.bonus_sugar_h_percent}% Sugar Earn Rate";

                imgCharName.LoadSpriteAutoParseAsync("name_" + girlId);
                this.HideProcessing();
            });
            // objEffectLight.SetActive(true);
            anim.Play();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SpecialExtensionShop.RecheckOffer().Forget();
        }
    }

}