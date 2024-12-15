using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class ShopCellViewPeriod : ESCellView<ModelApiShopData>
	{
		[SerializeField] private ShopPurchase shopPurchase;
		[SerializeField] private ItemTimer timerDuration;
		[SerializeField] private Image imageBackground;
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private List<Color> mainColors;

		public override void SetData(ModelApiShopData data)
		{
			var textIds = new Dictionary<TypeShopItem, string>
			{
				{TypeShopItem.Daily1, Localization.Get(TextId.Shop_Daily1)},
				{TypeShopItem.Daily2, Localization.Get(TextId.Shop_Daily2) + " 1"},
				{TypeShopItem.Daily21, Localization.Get(TextId.Shop_Daily2) + " 2"},
				{TypeShopItem.Daily3, Localization.Get(TextId.Shop_Daily3) + " 1"},
				{TypeShopItem.Daily31, Localization.Get(TextId.Shop_Daily3) + " 2"},
				{TypeShopItem.Daily4, Localization.Get(TextId.Shop_Daily4)},
				{TypeShopItem.Daily5, Localization.Get(TextId.Shop_Daily5)},
				{TypeShopItem.Weekly1, Localization.Get(TextId.Shop_Weekly1)},
				{TypeShopItem.Weekly2, Localization.Get(TextId.Shop_Weekly2)},
			};

			var type = data.GetItemType();
			var index = GameUtils.GetPackageIndexByType(type) - 1;

			// imageBackground.sprite = ControllerSprite.Instance.GetPackageBg(type);
			textTitle.text = textIds[type].ToUpper();
			textTitle.color = mainColors[index];

			var duration = data.reset_time - ServiceTime.CurrentUnixTime;
			timerDuration.SetDuration(duration);

			shopPurchase.SetData(data);
		}
	}
}