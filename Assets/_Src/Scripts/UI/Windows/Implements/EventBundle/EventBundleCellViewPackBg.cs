
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class EventBundleCellViewPackBg : ABundleCellViewWithBanner
    {
        [SerializeField] private TMP_Text txtDes;
        
        public override void SetData(AModelEventBundleCellView data)
        {
            base.SetData(data);
            if (data is ModelEventBundleCellViewPackBg modelData)
            {
                imgBanner.LoadSpriteAutoParseAsync($"banner_{modelData.EventId}_bg");
                txtCurrentPrice.text = ((int) modelData.DataBundle.GetFinalPrice()).ToFormat();
                txtDes.text = ExtensionEnum.ToEventBundleName(data.Type, modelData.EventId);
                objSoldOut.SetActive(modelData.DataBundle.IsReachLimit);
                btnClick.interactable = !modelData.DataBundle.IsReachLimit;
            }
        }
    }
}