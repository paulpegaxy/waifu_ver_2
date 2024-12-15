using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
    [Factory(ApiType.User, true)]
    public class ApiUser : Api<ModelApiUser>
    {
        public async UniTask<ModelApiUser> Get()
        {
            var data = await Get<ModelApiUser>($"/v1/user/my-info", "data");
            Data.User = data.User;
            // Data.Game = data.Game;
            Data.Notification();
            return data;
        }

        public async UniTask<List<ModelApiUserSubscriptionData>> GetSubscriptions()
        {
            var data = await Get<List<ModelApiUserSubscriptionData>>("/v1/chat/subscription", "data.dataSubscription");
            Data.Subscriptions = data;
            Data.Notification();
            return data;
        }

        public async UniTask PostGetTrialBot()
        {
            await Post<string>("/v1/game/trial-bot", "status", new { });
            await Get();
        }
        
        public async UniTask<List<ModelApiUserTonHistoryData>> TonHistories()
        {
            return await Get<List<ModelApiUserTonHistoryData>>($"/v1/user/ton-histories", "data");
        }

        public async UniTask<List<ModelApiUserTonTransactionData>> TonTransactions()
        {
            return await Get<List<ModelApiUserTonTransactionData>>($"/v1/user/ton-transactions", "data");
        }
        
        public async UniTask<ModelApiClaim> SignWithdrawTon(string to_address)
        {
            return await Post<ModelApiClaim>($"/v1/user/sign-withdraw-ton", "data", new { to_address });
        }

        public async UniTask<ModelApiUser> CompleteWithdrawTon(int id, string tx_hash)
        {
            return await Post<ModelApiUser>($"/v1/user/complete-withdraw-ton", "data", new { id, tx_hash });
        }

        public async UniTask<bool> ResetAccount()
        {
            return await Post<bool>("/v1/game/reset-data", "status");
        }
        
        public async UniTask<ModelApiUserSubscribe> PostBuySubscription(string sub_id)
        {
            return await Post<ModelApiUserSubscribe>($"/v1/user/telegram-subscription", "data", new {sub_id});
        }

        public async UniTask<ModelApiUserSubscribe> Subscribe()
        {
            return await Post<ModelApiUserSubscribe>($"/v1/user/telegram-subscription", "data");
        }
    }
}