using System;
using System.Collections.Generic;
using BreakInfinity;
using Newtonsoft.Json;
using Template.Defines;
using UnityEngine.Serialization;

namespace Game.Model
{
	[Serializable]
	public class ModelApiFriendConfig
	{
		public float total_usd_iap;
		public int total_invited;
		public string total_berry;
		public List<ModelApiFriendConfigInvited> my_invited_summary;
		public List<ModelApiFriendConfigLayer> referral_layer_config;
		public List<ModelFriendConfigInviteBonus> referral_invite_bonus_config;
		public List<ModelFriendConfigFriendBonus> referral_friend_bonus_config;
		public List<ModelFriendConfigClaim> buddy_tree_claim_time_config;
		
		public List<ModelFriendConfigProfit> profits;
		// public List<ModelFriendConfigInvitedBonusData> referral_invited_bonus_config_new;
		
		// public List<ModelAPoi>

		public BigDouble TotalBerryParse => BigDouble.Parse(total_berry);

		public int GetInvitedByLayer(int layer)
		{
			var item = my_invited_summary.Find(x => x.layer == layer);
			return item != null ? item.count : 0;
		}

		public float GetProfit(TypeResource resourceType, int layer)
		{
			var item = profits.Find(x => x.layer == layer && x.typeResource == resourceType);
			return item?.amount ?? 0;
		}
		
		public bool IsAvailableToClaim()
		{
			return buddy_tree_claim_time_config.Find(x => ServiceTime.CurrentUnixTime >= x.time_start && ServiceTime.CurrentUnixTime <= x.time_end) != null;
		}
	}

	[Serializable]
	public class ModelApiFriendConfigInvited
	{
		public int layer;
		public int count;
	}

	[Serializable]
	public class ModelApiFriendConfigLayer
	{
		public int layer;
		public float iap;
		public int invite;
		public int invite_level;
		public int benefit;
	}

	[Serializable]
	public class ModelFriendConfigInviteBonus
	{
		public int normal_friend;
		public int telegram_premium_friend;
	}

	[Serializable]
	public class ModelFriendConfigInvitedBonusData
	{
		public int rank_of_invited_friend;
		public int regular_friend_berry;
		public int telegram_premium_friend_berry;
	}

	[Serializable]
	public class ModelFriendConfigFriendBonus
	{
		[JsonProperty("rank_of_invited_friend")]
		public TypeLeagueCharacter league;
		[JsonProperty("regular_friend")]
		public int normal_friend;
		public int telegram_premium_friend;
		
		public int GetFinalBonus(bool isPremium)
		{
			return isPremium ? telegram_premium_friend : normal_friend;
		}
	}

	[Serializable]
	public class ModelFriendConfigProfit
	{
		public int user_id;
		public int layer;
		public string currency;
		public float amount;
	
		
		[JsonProperty("ingame_id")]
		public TypeResource typeResource;
	}
	
	[Serializable]
	public class ModelFriendConfigClaim
	{
		public int id;
		public long time_start;
		public long time_end;
	}
}