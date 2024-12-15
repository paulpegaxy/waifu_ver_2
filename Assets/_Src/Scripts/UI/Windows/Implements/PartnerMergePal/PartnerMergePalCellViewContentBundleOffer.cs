using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalCellViewContentBundleOffer : ESCellView<AModelPartnerMergePalCellView>
    {
        [SerializeField] private ShopCellViewOfferPack itemOfferPack;

        public override void SetData(AModelPartnerMergePalCellView data)
        {
            if (data is ModelPartnerMergePalCellViewContentBundleOffer modelData)
            {
                itemOfferPack.SetData(modelData.Data);
            }
        }
    }
}