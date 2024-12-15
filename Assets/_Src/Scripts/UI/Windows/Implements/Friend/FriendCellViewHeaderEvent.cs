using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Model;
using Game.Runtime;
using Game.Defines;
using Template.Defines;

namespace Game.UI
{
	public class FriendCellViewHeaderEvent : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private ItemTimerAutoLabel itemTimer;
		[SerializeField] private FriendEventSeason friendSeason;
		[SerializeField] private GameObject objectTimer;
		[SerializeField] private GameObject objectReward;
		[SerializeField] private GameObject objectNowOn;
		[SerializeField] private TMP_Text textLast;
		[SerializeField] private TMP_Text textPrev;
		[SerializeField] private TMP_Text textCurrent;
		[SerializeField] private TMP_Text textTon;
		[SerializeField] private TMP_Text textPal;
		[SerializeField] private TMP_Text textSc;
		[SerializeField] private TMP_Text textSlogan;

		private readonly Dictionary<TypeFriendSeason, GameObject> _objectSeasons = new();
		private readonly Dictionary<TypeFriendSeason, TMP_Text> _textSeasons = new();
		private readonly Dictionary<TypeResource, TMP_Text> _textRewards = new();
		private ModelFriendCellViewHeaderEvent _data;

		private void Awake()
		{
			_objectSeasons[TypeFriendSeason.Last] = textLast.transform.parent.gameObject;
			_objectSeasons[TypeFriendSeason.Prev] = textPrev.transform.parent.gameObject;
			_objectSeasons[TypeFriendSeason.Current] = textCurrent.transform.parent.gameObject;

			_textSeasons[TypeFriendSeason.Last] = textLast;
			_textSeasons[TypeFriendSeason.Prev] = textPrev;
			_textSeasons[TypeFriendSeason.Current] = textCurrent;

			_textRewards[TypeResource.Ton] = textTon;
			_textRewards[TypeResource.Berry] = textPal;
			_textRewards[TypeResource.HeartPoint] = textSc;
		}

		private void OnEnable()
		{
			FriendEventSeason.OnChanged += OnSeasonChange;
		}

		private void OnDisable()
		{
			FriendEventSeason.OnChanged -= OnSeasonChange;
		}

		private void OnSeasonChange(TypeFriendSeason season)
		{
			foreach (var objectSeason in _objectSeasons)
			{
				var config = _data.Config.GetData(objectSeason.Key);
				objectSeason.Value.SetActive(config != null);

				if (config != null)
				{
					_textSeasons[objectSeason.Key].text = $"{Localization.Get(TextId.Common_Season)} {config.season_number}";
				}
			}
			objectNowOn.SetActive(_data.Config.GetData(TypeFriendSeason.Last) != null || _data.Config.GetData(TypeFriendSeason.Prev) != null);
		}

		public override void SetData(ModelFriendCellView model)
		{
			_data = model as ModelFriendCellViewHeaderEvent;

			var config = _data.Config.GetData(_data.Season);
			var duration = config.time_end - ServiceTime.CurrentUnixTime;
			var isEnded = duration <= 0;
			if (!isEnded) itemTimer.SetDuration(duration);

			textSlogan.text = Localization.Get(config.IsEnded ? TextId.Common_EventEnded : TextId.Friend_EventSlogan);

			objectTimer.SetActive(!isEnded);
			objectReward.SetActive(!isEnded);
			friendSeason.SetData(_data.Season);

			_textRewards[TypeResource.Ton].transform.parent.gameObject.SetActive(false);
			_textRewards[TypeResource.Berry].transform.parent.gameObject.SetActive(false);
			_textRewards[TypeResource.HeartPoint].transform.parent.gameObject.SetActive(false);

			foreach (var reward in _data.Config.total_rewards)
			{
				_textRewards[reward.IdResource].transform.parent.gameObject.SetActive(true);
				if (reward.IdResource == TypeResource.HeartPoint)
					_textRewards[reward.IdResource].text = reward.QuantityParse.ToLetter();
				else
					_textRewards[reward.IdResource].text = reward.QuantityParse.ToString();
			}
		}
	}
}
