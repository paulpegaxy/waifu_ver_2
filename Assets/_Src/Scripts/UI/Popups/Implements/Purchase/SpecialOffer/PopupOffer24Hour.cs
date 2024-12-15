using Game.Model;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PopupOffer24Hour : MonoBehaviour
    {
        [SerializeField] private ShopCellViewOfferDay24Hour information;

        public void SetData(ModelApiShopData data)
        {
            information.SetData(new ModelShopOfferCellView()
            {
                Type = ShopOfferType.Day24Hour,
                ShopItem = data
            });
        }   
    }
}