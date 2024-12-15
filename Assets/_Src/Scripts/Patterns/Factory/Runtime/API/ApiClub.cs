using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
    [Factory(ApiType.Club, true)]
    public class ApiClub : Api<ModelApiClub>
    {
        public async UniTask<ModelApiClubAll> Get(int offset = 0, int limit = 20, bool random = true)
        {
            var clubs = await Get<ModelApiClubAll>($"/v1/club/all", "data", new { offset, limit, random });

            // var club = new ModelApiClubAll();
            // club.MockUp();
            
            Data.Clubs = clubs;
            Data.Notification();

            return clubs;
        }

        public async UniTask<ModelApiClubData> GetClub(int club_id)
        {
            // return Data.Clubs.data[0];
            return await Get<ModelApiClubData>($"/v1/club/detail/{club_id}", "data");
        }

        public async UniTask<ModelApiClubData> Join(int club_id)
        {
            var data = await Post<ModelApiClubData>($"/v1/club/join-club", "data", new { club_id });
            var apiUser = FactoryApi.Get<ApiUser>();

            apiUser.Data.Club = data;
            apiUser.Data.Notification();

            return data;
        }

        public async UniTask<ModelApiClubData> Leave(int club_id)
        {
            var data = await Post<ModelApiClubData>($"/v1/club/leave-club", "data", new { club_id });
            var apiUser = FactoryApi.Get<ApiUser>();

            apiUser.Data.Club = null;
            apiUser.Data.Notification();

            return data;
        }

        public async UniTask<List<ModelApiClubBoostInfo>> GetBoostPrices(int club_id)
        {
            return await Get<List<ModelApiClubBoostInfo>>($"/v1/club/{club_id}/boost-price", "data");
        }
    }
}