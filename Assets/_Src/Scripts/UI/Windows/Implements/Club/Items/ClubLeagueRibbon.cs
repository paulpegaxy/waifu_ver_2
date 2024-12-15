using System;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
	public class ClubLeagueRibbon : MonoBehaviour
	{
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private Image imageLeague;
		[SerializeField] private Image imageRibbon;
		[SerializeField] private UIButton buttonView;

		public static Action<ModelApiClubData> OnView;

		private ModelApiClubData _data;

		private void OnEnable()
		{
			buttonView.onClickEvent.AddListener(OnViewClick);
		}

		private void OnDisable()
		{
			buttonView.onClickEvent.RemoveListener(OnViewClick);
		}

		private void OnViewClick()
		{
			OnView?.Invoke(_data);
		}

		public void SetData(ModelApiClubData data)
		{
			// var league = data.league;
			// textTitle.text = $"{league} {Localization.Get(TextId.Common_League)}";
			// imageLeague.sprite = ControllerSprite.Instance.GetLeagueIconBig(league);
			// imageRibbon.sprite = ControllerSprite.Instance.GetLeagueRibbon(league);

			_data = data;
		}
	}

}
