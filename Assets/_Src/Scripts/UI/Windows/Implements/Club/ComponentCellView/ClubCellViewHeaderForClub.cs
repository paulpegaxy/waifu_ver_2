using UnityEngine;
using TMPro;
using BreakInfinity;
using Game.Model;
using Game.Runtime;
using UnityEngine.Serialization;

namespace Game.UI
{
	public class ClubCellViewHeaderForClub : ESCellView<ModelClubCellView>
	{
		[SerializeField] private GameObject objEmptyClub;
		[SerializeField] private ClubConfig clubConfig;
		[SerializeField] private ClubLeague clubLeauge;
		[FormerlySerializedAs("aItemFilterType")] [FormerlySerializedAs("itemFilterType")] [SerializeField] private ClubFilterType clubFilterType;
		// [SerializeField] private ClubFilterTime clubFilterTime;

		private ModelApiClubData _myClubData;
		
		private string GetSubTitle(ModelClubCellViewHeaderForClub data)
		{
			var apiLeaderboard = FactoryApi.Get<ApiLeaderboard>();
			var filter = data.Filter;
			_myClubData = FactoryApi.Get<ApiUser>().Data.Club;
			// var config = apiLeaderboard.Data.Config.ConfigClub.Find(x => 
			// 	x.league == data.Filter.typeLeagueIndex && x.type == filter.FilterType);
			apiLeaderboard.Data.Config.GetConfigClub((TypeLeague)filter.typeLeagueIndex,out ModelApiLeaderboardConfigData config);

			if (config != null)
			{
				BigDouble myScore = 0;
				if (filter.FilterType == FilterType.Personal)
				{
					myScore = BigDouble.Parse(data.My.points);
				}
				else
				{
					var apiUser = FactoryApi.Get<ApiUser>();
					var club = apiUser.Data.Club;

					if (club != null)
					{
						myScore = club.TotalPointParse;
					}
				}

				var minNumber = BigDouble.Parse(config.from_point);
				var maxNumber = config.to_point != "Infinity" ? BigDouble.Parse(config.to_point) : BigDouble.PositiveInfinity;

				if (myScore > 0 && myScore >= minNumber && myScore <= maxNumber)
				{
					return $"{myScore.ToLetter()}/{maxNumber.ToLetter()}";
				}
				else
				{
					return $"{Localization.Get(TextId.Common_From)} {minNumber.ToLetter()}";
				}
			}

			return string.Empty;
		}

		public override void SetData(ModelClubCellView model)
		{
			var data = model as ModelClubCellViewHeaderForClub;
			var filter = data.Filter;
			//
			// var typeLeague= (TypeLeague)filter.typeLeagueIndex;
			//
			// clubLeauge.SetData(typeLeague);
			clubFilterType.SetData(filter.FilterType);
			// _myClubData = FactoryApi.Get<ApiUser>().Data.Club;
			// if (_myClubData != null)
			// {
			// 	clubConfig.gameObject.SetActive(true);
			// 	objEmptyClub.SetActive(false);
			// 	clubConfig.SetData(_myClubData);
			// }
			// else
			// {
			// 	clubConfig.gameObject.SetActive(false);
			// 	objEmptyClub.SetActive(true);
			// }
		}
	}
}
