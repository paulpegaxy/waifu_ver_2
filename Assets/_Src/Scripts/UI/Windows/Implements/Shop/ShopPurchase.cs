using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
	public class ShopPurchase : MonoBehaviour
	{
		[SerializeField] private TMP_Text textPal;
		[SerializeField] private TMP_Text textBag;
		[SerializeField] private TMP_Text textGold;
		[SerializeField] private TMP_Text textPrice;
		[SerializeField] private TMP_Text textBuyed;
		[SerializeField] private UIButton buttonBuy;
		[SerializeField] private GameObject objectBuyed;

		private ModelApiShopData _data;
		private Dictionary<TypeResource, TMP_Text> _texts = new();

		protected virtual void Awake()
		{
			_texts.Add(TypeResource.Berry, textPal);
			// _texts.Add(TypeResource.Bag, textBag);
			_texts.Add(TypeResource.HeartPoint, textGold);
		}

		protected virtual void OnEnable()
		{
			buttonBuy.onClickEvent.AddListener(OnBuyClick);
		}

		protected virtual void OnDisable()
		{
			buttonBuy.onClickEvent.RemoveListener(OnBuyClick);
		}

		private async void OnBuyClick()
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiShop = FactoryApi.Get<ApiShop>();
				var data = await apiShop.BuyWithTon(_data.id);

				foreach (var item in _data.items)
				{
					ControllerResource.Add(item.IdResource, item.QuantityParse);
					ControllerUI.Instance.Spawn(item.IdResource, GetCurrencyPosition(item.IdResource), 20);
				}

				_data.purchased_count++;
				apiShop.Data.Notification();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);
		}

		public void SetData(ModelApiShopData data)
		{
			SetVisible(TypeResource.Berry, false);
			// SetVisible(ResourceType.Bag, false);
			SetVisible(TypeResource.HeartPoint, false);

			foreach (var item in data.items)
			{
				if (_texts[item.IdResource] == null) continue;

				if (item.IdResource == TypeResource.HeartPoint)
				{
					_texts[item.IdResource].text = $"+{item.QuantityParse.ToLetter()}";
				}
				else
				{
					_texts[item.IdResource].text = $"+{item.QuantityParse}";
				}

				SetVisible(item.IdResource, true);
			}

			if (textPrice != null) textPrice.text = $"${data.GetFinalPrice().ToDigit()}";
			if (textBuyed != null) textBuyed.text = Localization.Get(TextId.Shop_SoldOut);

			buttonBuy.gameObject.SetActive(!data.IsReachLimit);
			objectBuyed.SetActive(data.IsReachLimit);

			_data = data;
		}

		public void SetBuy(bool value)
		{
			buttonBuy.gameObject.SetActive(value);
			objectBuyed.SetActive(!value);
		}

		public void SetBuyedDescription(string description)
		{
			textBuyed.text = description;
		}

		private void SetVisible(TypeResource type, bool value)
		{
			if (!_texts.ContainsKey(type) || _texts[type] == null) return;
			_texts[type].transform.parent.gameObject.SetActive(value);
		}

		private Vector3 GetCurrencyPosition(TypeResource type)
		{
			return type switch
			{
				TypeResource.Berry => textPal.transform.position,
				// TypeResource.Bag => textBag.transform.position,
				TypeResource.HeartPoint => textGold.transform.position,
				_ => Vector3.zero,
			};
		}
	}
}