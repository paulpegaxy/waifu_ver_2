using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.Signals;
using TMPro;
using BreakInfinity;
using Game.Runtime;
using Game.Core;
using Game.Model;

namespace Game.UI
{
	public class ClubHeader : MonoBehaviour
	{
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text textScore;
		[SerializeField] private TMP_Text textLeague;
		[SerializeField] private Image imageLeague;
		[SerializeField] private UIButton buttonJoin;
		[SerializeField] private UIButton buttonView;

		private void OnEnable()
		{
			ModelApiUser.OnChanged += OnUserInfoChanged;
			buttonJoin.onClickEvent.AddListener(OnJoin);
			buttonView.onClickEvent.AddListener(OnView);

			Refresh();
		}

		private void OnDisable()
		{
			ModelApiUser.OnChanged -= OnUserInfoChanged;
			buttonJoin.onClickEvent.RemoveListener(OnJoin);
			buttonView.onClickEvent.RemoveListener(OnView);
		}

		private void OnUserInfoChanged(ModelApiUser userInfo)
		{
			Refresh();
		}

		private void OnJoin()
		{
			// if (!Utils.IsFeatureUnlocked())
			// {
			// 	ControllerPopup.ShowToast(TextId.Toast_LevelUpPet);
			// 	return;
			// }

			Signal.Send(StreamId.UI.ClubRandom);
		}

		private void OnRanking()
		{
			/*
			var apiUser = FactoryApi.Get<ApiUser>();
			var userInfo = apiUser.Data;

			EventManager.EmitEvent(GameEventType.Club, new ModelClubFilter()
			{
				FilterType = FilterType.Club,
				FilterTimeType = FilterTimeType.Day,
				LeagueType = userInfo.Club.league,
			});

			Signal.Send(StreamId.UI.Club);
			*/
		}

		private void OnView()
		{
			var apiUser = FactoryApi.Get<ApiUser>();
			var userInfo = apiUser.Data;

			if (userInfo.Club != null)
			{
				ClubWindow.OpenDetail(userInfo.Club.id);
			}
		}

		private void Refresh()
		{
			var apiUser = FactoryApi.Get<ApiUser>();
			var userInfo = apiUser.Data;

			if (userInfo.Club != null)
			{
				textName.text = userInfo.Club.name;
				textScore.text = userInfo.Club.TotalPointParse.ToLetter();
				textLeague.text = userInfo.Club.league.ToString();
				textLeague.color = userInfo.Club.league.ToColor();
				// imageLeague.sprite = ControllerSprite.Instance.GetLeagueIcon(userInfo.Club.league);
				imageLeague.LoadSpriteAutoParseAsync("league_" + (int)userInfo.Club.league);
			}

			buttonView.gameObject.SetActive(userInfo.Club != null);
			buttonJoin.gameObject.SetActive(userInfo.Club == null);
		}
	}
}
