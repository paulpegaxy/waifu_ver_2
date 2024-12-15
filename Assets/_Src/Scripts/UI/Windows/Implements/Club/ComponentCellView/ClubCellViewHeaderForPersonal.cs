// Author: ad   -
// Created: 14/08/2024  : : 01:08
// DateUpdate: 14/08/2024

using UnityEngine;
using TMPro;
using BreakInfinity;
using Game.Model;
using Game.Runtime;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI
{
	public class ClubCellViewHeaderForPersonal : ESCellView<ModelClubCellView>
	{
		[SerializeField] private ClubConfigPersonal clubConfig;
		[SerializeField] private ClubLeagueGirl clubLeague;
		[SerializeField] private ClubFilterType clubFilterType;
		// [SerializeField] private ClubFilterTime clubFilterTime;

		private string GetSubTitle(ModelClubCellViewHeaderForPersonal data)
		{
			var apiLeaderboard = FactoryApi.Get<ApiLeaderboard>();
			var filter = data.Filter;
			
			var config = apiLeaderboard.Data.Config.ConfigPersonal.Find(x => 
				x.league == data.Filter.typeLeagueIndex && x.type == filter.FilterType);

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
			var data = model as ModelClubCellViewHeaderForPersonal;
			var filter = data.Filter;
			
			var typeFilter = (TypeLeagueCharacter)(filter.typeLeagueIndex);
			
			//TODO: Will change when increase char. Temp processing
			if ((int)typeFilter > (int)TypeLeagueCharacter.Char_5)
				typeFilter = TypeLeagueCharacter.Char_5;
			clubLeague.SetData(typeFilter);
			
			clubFilterType.SetData(filter.FilterType);
			clubConfig.SetData(data.My, typeFilter);

			// clubFilterTime.SetData(filter.FilterTimeType);
			// clubFilterTime.ShowAllTime(filter.FilterType == FilterType.Personal);
		}
	}
}
