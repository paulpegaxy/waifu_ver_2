using UnityEngine;
using TMPro;
using Game.Model;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Defines;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class FriendCellViewContentEvent : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private ItemRanking itemRanking;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text textFriendCount;
		[SerializeField] private TMP_Text textTon;
		[SerializeField] private TMP_Text txtHc;
		[SerializeField] private TMP_Text txtSc;

		private Dictionary<TypeResource, TMP_Text> _texts = new();

		private void Awake()
		{
			_texts.Add(TypeResource.Ton, textTon);
			_texts.Add(TypeResource.Berry, txtHc);
			_texts.Add(TypeResource.HeartPoint, txtSc);
		}

		public override void SetData(ModelFriendCellView model)
		{
			var data = model as ModelFriendCellViewContentEvent;
			var item = data.Item;

			itemRanking.SetData(item.rank, item.user.name, data.IsMyRank);
			textName.text = item.user.name;
			textFriendCount.text = $"+{item.invited} {Localization.Get(TextId.Common_Friends)}";

			SetVisible(TypeResource.Ton, false);
			SetVisible(TypeResource.Berry, false);
			SetVisible(TypeResource.HeartPoint, false);

			foreach (var resource in item.items)
			{
				if (_texts[resource.IdResource] == null) continue;

				if (resource.IdResource == TypeResource.HeartPoint)
				{
					_texts[resource.IdResource].text = $"+{resource.QuantityParse.ToLetter()}";
				}
				else
				{
					_texts[resource.IdResource].text = $"+{resource.QuantityParse}";
				}

				SetVisible(resource.IdResource, true);
			}
		}

		private void SetVisible(TypeResource type, bool value)
		{
			if (!_texts.ContainsKey(type) || _texts[type] == null) return;
			_texts[type].transform.parent.gameObject.SetActive(value);
		}
	}
}
