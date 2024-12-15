using UnityEngine;
using TMPro;
using Game.Model;
using UnityEngine.UI;
using Game.Runtime;
using System.Collections.Generic;

namespace Game.UI
{
	public class PartnerCellViewPackage : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private ShopPurchase shopPurchase;
		[SerializeField] private Image imageBackground;
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private TMP_Text textPriceFake;
		[SerializeField] private TMP_Text textSaleOff;

		public override void SetData(ModelPartnerCellView model)
		{
			var data = model as ModelPartnerCellViewPackage;
			var shopData = data.ShopData;
			var textIds = new List<TextId>()
			{
				// TextId.Partner_Offer1,
				// TextId.Partner_Offer2,
				// TextId.Partner_Offer3,
				// TextId.Partner_Offer4,
				// TextId.Partner_Offer5,
			};

			imageBackground.color = GameUtils.GetPartnerPackageColorBg(data.Index - 1);
			textTitle.text = Localization.Get(textIds[data.Index - 1]).ToUpperCase();
			textTitle.color = GameUtils.GetPartnerPackageColorTitle(data.Index - 1);
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