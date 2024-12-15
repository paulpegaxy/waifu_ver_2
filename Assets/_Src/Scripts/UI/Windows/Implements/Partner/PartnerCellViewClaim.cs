using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class PartnerCellViewClaim : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private TMP_Text textReward;
		[SerializeField] private TMP_Text textCurrent;
		[SerializeField] private TMP_Text textTotal;
		[SerializeField] private UIButton buttonClaim;
		[SerializeField] private GameObject objectClaimInactive;
		[SerializeField] private GameObject objectClaimed;

		private ModelPartnerCellViewClaim _data;
		private ModelApiQuestData _mainQuest;

		private void OnEnable()
		{
			ModelApiQuest.OnChanged += OnChanged;
			buttonClaim.onClickEvent.AddListener(OnClaim);
		}

		private void OnDisable()
		{
			ModelApiQuest.OnChanged -= OnChanged;
			buttonClaim.onClickEvent.RemoveListener(OnClaim);
		}

		private void OnChanged(ModelApiQuest data)
		{
			var quests = data.Quest.FindAll(x => x.category == _data.Config.quest_type);
			var processed = quests.FindAll(x => x.claimed).Count;

			_mainQuest.processed = processed;
			if (_mainQuest.processed >= _mainQuest.process)
			{
				_mainQuest.can_claim = true;
			}

			SetData(_data);
		}

		private void OnClaim()
		{
			if (_mainQuest == null) return;
			QuestCellViewContentQuest.OnClaim?.Invoke(new ModelQuestCellViewContentQuest() { Quest = _mainQuest }, transform.position);
		}

		public override void SetData(ModelPartnerCellView model)
		{
			var apiEvent = FactoryApi.Get<ApiEvent>();
			var apiQuest = FactoryApi.Get<ApiQuest>();
			var data = model as ModelPartnerCellViewClaim;

			if (_mainQuest == null)
			{
				_mainQuest = apiQuest.Data.Quest.Find(x => x.category == $"{data.Config.quest_type}Reward");
				if (_mainQuest == null) return;
			}

			textReward.text = _mainQuest.items[0].QuantityParse.ToString();
			textCurrent.text = _mainQuest.processed.ToString();
			textTotal.text = _mainQuest.process.ToString();

			buttonClaim.gameObject.SetActive(_mainQuest.can_claim && !_mainQuest.claimed);
			objectClaimed.SetActive(_mainQuest.claimed);
			objectClaimInactive.SetActive(!_mainQuest.can_claim && !_mainQuest.claimed);

			_data = data;
		}
	}
}