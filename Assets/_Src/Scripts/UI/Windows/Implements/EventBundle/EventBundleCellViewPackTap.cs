using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EventBundleCellViewPackTap :  ABundleCellViewWithBanner
    {
        [SerializeField] private TMP_Text txtDes;
        
        public override void SetData(AModelEventBundleCellView data)
        {
            base.SetData(data);
            if (data is ModelEventBundleCellViewPackTap modelData)
            {
                imgBanner.LoadSpriteAutoParseAsync($"banner_{modelData.EventId}_tap");
                txtCurrentPrice.text = ((int) modelData.DataBundle.GetFinalPrice()).ToFormat();
                txtDes.text = ExtensionEnum.ToEventBundleName(data.Type, modelData.EventId);
                objSoldOut.SetActive(modelData.DataBundle.IsReachLimit);
                btnClick.interactable = !modelData.DataBundle.IsReachLimit;
            }
        }
    }
}