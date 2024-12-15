using System;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EventBundleCellViewPackOfferTimelapse :  ABundleCellViewWithBanner
    {
        [SerializeField] private TMP_Text txtScReceive;
        [SerializeField] private TMP_Text txtHcReceive;
        [SerializeField] private TMP_Text txtLimit;
        [SerializeField] private TMP_Text txtDesTimelapse;
        
        
        public override void SetData(AModelEventBundleCellView data)
        {
            base.SetData(data);
            if (data is ModelEventBundleCellViewPackOfferTimelapse modelData)
            {
                imgBanner.LoadSpriteAutoParseAsync($"banner_{modelData.EventId}_offer");
                txtLimit.text =
                    $"{Localization.Get(TextId.Shop_Limit)} {modelData.DataBundle.purchased_count}/{modelData.DataBundle.limit}";
                string highlightValueStr = "8".SetHighlightStringOrange();
                txtDesTimelapse.text = string.Format(Localization.Get(TextId.Shop_DesTimelapse), highlightValueStr.Replace("h", ""));
                objSoldOut.SetActive(modelData.DataBundle.IsReachLimit);
                btnClick.interactable = !modelData.DataBundle.IsReachLimit;

                txtCurrentPrice.text = "$" + modelData.DataBundle.GetFinalPrice().ToDigit();
                txtOriginPrice.text = "$" + modelData.DataBundle.price.ToDigit();
                
                var rewardData = modelData.DataBundle.items;
                txtHcReceive.text = rewardData[0].ValueParse.ToLetter();
                txtScReceive.text = rewardData[1].ValueParse.ToLetter();
            }
        }
    }
}