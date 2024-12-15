using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
	[Factory(ApiType.Friend, true)]
	public class ApiFriend : Api<ModelApiFriend>
	{
		public async UniTask<ModelApiFriendConfig> Get()
		{
			var data = await Get<ModelApiFriendConfig>("/v1/referral/my", "data");

			Data.Config = data;
			Data.Notification();

			return data;
		}

		public async UniTask<ModelApiFriendInvited> GetFriends(int offset = 0, int limit = 50)
		{
			var data = await Get<ModelApiFriendInvited>("/v1/referral/my-invited", "data", new { offset, limit });

			Data.Invited = data;
			Data.Notification();

			return data;
		}

		public async UniTask<ModelApiFriendEventConfig> EventConfig()
		{
			var config = await Get<ModelApiFriendEventConfig>("/v1/referral/event/config", "data");
			Data.EventConfig = config;

			return config;
		}

		public async UniTask<ModelApiFriendEventLeaderboard> EventLeaderboard(int season)
		{
			var leaderboard = await Get<ModelApiFriendEventLeaderboard>($"/v1/referral/event/{season}/leaderboard", "data");
			Data.EventLeaderboard = leaderboard;
			Data.Notification();

			return leaderboard;
		}

		public async UniTask<ModelApiFriendLeaderboard> GetLeaderboard(int offset = 0, int limit = 300, FriendSortBy sortBy = FriendSortBy.TotalBerry, FriendSortType sortType = FriendSortType.Desc)
		{
			var sort_by = sortBy.ToString().PascalToSnake();
			var sort_type = sortType.ToString().ToUpperCase();

			var data = await Get<ModelApiFriendLeaderboard>("/v1/referral/leaderboard", "data",
				new { offset, limit, sort_type, sort_by });

			return data;
		}

		public async UniTask PostSlapMyFriend(int id)
		{
			await Post<string>("/v1/referral/slap", "status", new { friend_user_id = id });
			await GetFriends();
		}

		public async UniTask<ModelApiClaim> ClaimBuddyTree()
		{
			return await Post<ModelApiClaim>("/v1/referral/claim-profit", "data", new { });
		}
	}
}