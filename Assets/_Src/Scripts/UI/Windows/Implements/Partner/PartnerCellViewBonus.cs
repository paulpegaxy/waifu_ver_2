using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;
using Game.Defines;
using Game.Runtime;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
	public class PartnerCellViewBonus : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private TMP_Text txtHc;
		[SerializeField] private UIButton buttonClaim;
		[SerializeField] private GameObject objectClaimed;

		private Dictionary<TypeResource, TMP_Text> _texts = new();
		private ModelPartnerCellViewBonus _data;

		protected virtual void Awake()
		{
			_texts.Add(TypeResource.Berry, txtHc);
		}

		protected void OnEnable()
		{
			buttonClaim.onClickEvent.AddListener(OnClaimClick);
		}

		protected void OnDisable()
		{
			buttonClaim.onClickEvent.RemoveListener(OnClaimClick);
		}

		private async void OnClaimClick()
		{
			var apiEvent = FactoryApi.Get<ApiEvent>();
			var config = _data.Config;

			await apiEvent.ClaimGift(config.id);
			
			config.claimed = true;
			apiEvent.Data.Notification();

			foreach (var item in config.items)
			{
				ControllerResource.Add(item.IdResource, item.QuantityParse);
				ControllerUI.Instance.Spawn(item.IdResource, GetCurrencyPosition(item.IdResource), 20);
			}
		}

		public override void SetData(ModelPartnerCellView model)
		{
			var data = model as ModelPartnerCellViewBonus;
			var config = data.Config;

			SetVisible(TypeResource.Berry, false);

			foreach (var item in config.items)
			{
				if (_texts[item.IdResource] == null) continue;
				_texts[item.IdResource].text = $"+{item.QuantityParse}";

				SetVisible(item.IdResource, true);
			}

			// textBoost.text = $"x{config.boost}\n{Localization.Get(TextId.Common_GoldBoost)}";

			buttonClaim.gameObject.SetActive(!config.claimed);
			objectClaimed.SetActive(!buttonClaim.gameObject.activeSelf);

			_data = data;
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
				TypeResource.Berry => txtHc.transform.position,
				_ => Vector3.zero,
			};
		}
	}
}