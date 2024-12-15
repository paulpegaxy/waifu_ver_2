using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class PartnerCellViewPackageCollab : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private ShopPurchase shopPurchase;
		[SerializeField] private Image imageBackground;
		[SerializeField] private TMP_Text textPriceFake;
		[SerializeField] private TMP_Text textSaleOff;

		public override void SetData(ModelPartnerCellView model)
		{
			var data = model as ModelPartnerCellViewPackageCollab;
			var shopData = data.ShopData;

			textPriceFake.text = $"${shopData.config_price.ToDigit()}";
			textPriceFake.gameObject.SetActive(shopData.sale_off_percent > 0);
			textSaleOff.text = $"{shopData.sale_off_percent * 100}%";
			textSaleOff.transform.parent.gameObject.SetActive(shopData.sale_off_percent > 0);

			var canBuy = !shopData.IsReachLimit;
			shopPurchase.SetData(shopData);
			shopPurchase.SetBuy(canBuy);

			if (!canBuy)
			{
				shopPurchase.SetBuyedDescription(Localization.Get(TextId.Shop_SoldOut));
			}
		}
	}
}