using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Runtime;
using Game.Model;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
	public class ShopItem : MonoBehaviour
	{
		[SerializeField] private TMP_Text textValue;
		[SerializeField] private TMP_Text textBonus;
		[SerializeField] private TMP_Text textPrice;
		[SerializeField] private Image imageItem;
		[SerializeField] private GameObject objectTag;
		[SerializeField] private UIButton buttonBuy;

		private ModelApiShopData _data;

		private void OnEnable()
		{
			buttonBuy?.onClickEvent.AddListener(OnBuyClick);
		}

		private void OnDisable()
		{
			buttonBuy?.onClickEvent.RemoveListener(OnBuyClick);
		}

		private void OnBuyClick()
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirmPurchaseBerry.ToString());
			popup.GetComponent<PopupConfirmPurchaseBerry>().SetData(_data.id);
			popup.GetComponent<PopupConfirmPurchaseBerry>().SetItemPosition(transform.position);
			popup.Show();
		}

		public void SetData(ModelApiShopData data)
		{
			// textValue.text = $"{data.items[0].quantity} {Localization.Get(TextId.Common_HcName)}";
			textValue.text = data.items[0].ValueParse.ToLetter();
			textBonus.text = $"+{data.bonus_items[0].ValueParse.ToLetter()}";
			// textPrice.text = $"${data.price.ToDigit()}";
			textPrice.text = data.GetTokenStar.price.ToString();

			// int idInt = int.Parse(data.id.Split('_')[^1]);

			// imageItem.sprite = ControllerSprite.Instance.GetShopIcon(idInt.ToString());
			// ExtensionImage.LoadShopIconHc(imageItem, idInt);

			// imageItem.SetNativeSize();

			objectTag.SetActive(!data.IsReceivedBonus(data.id));

			_data = data;
		}
	}
}