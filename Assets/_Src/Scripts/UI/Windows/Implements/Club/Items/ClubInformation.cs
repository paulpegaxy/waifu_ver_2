using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using BreakInfinity;
using Game.Runtime;
using Game.Model;
using UnityEngine.UI;


namespace Game.UI
{
	public class ClubInformation : MonoBehaviour
	{
		[SerializeField] private ItemAvatar itemAvatar;
		[SerializeField] private ItemRankingCircle itemRanking;
		[SerializeField] private ClubLeagueRibbon leagueRibbon;
		[SerializeField] private ClubDetailInformation itemInformation;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private UIButton buttonChannel;
		[SerializeField] private UIButton buttonAvatar;

		private ModelApiClubData _data;

		private void OnEnable()
		{
			buttonAvatar.onClickEvent.AddListener(OnChannel);
			buttonChannel.onClickEvent.AddListener(OnChannel);
		}

		private void OnDisable()
		{
			buttonAvatar.onClickEvent.RemoveListener(OnChannel);
			buttonChannel.onClickEvent.RemoveListener(OnChannel);
		}

		private void OnChannel()
		{
			GameUtils.OpenLink(_data.telegram_link);
		}

		public void SetData(ModelApiClubData data)
		{
			leagueRibbon.SetData(data);
			textName.text = data.name;
			itemInformation.LoadData(data);
			itemAvatar.SetNameAvatar(data.name);
			itemRanking.SetData(data.rank_top);
			// imgIconRank.sprite = ControllerSprite.Instance.GetLeagueIcon(data.league);
			
			
			_data = data;
		}
	}

}
