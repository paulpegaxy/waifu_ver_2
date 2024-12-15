using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;

namespace Game.Runtime
{
    [Factory(ApiType.Event, true)]
    public class ApiEvent : Api<ModelApiEvent>
    {
        public async UniTask Get()
        {
            var data = await Get<ModelApiEvent>("/v1/game/events/my", "data");
            Data.event_data=data.event_data;
            Data.events = data.events;

            Data.Notification();
        }

        public async UniTask ClaimBackground()
        {
            await Post<string>("/v1/quests/claim-background-yuki", "status");
        }

        public async UniTask<List<ModelApiEventLeaderboard>> GetEventLeaderboard(string event_id)
        {
            return await Get<List<ModelApiEventLeaderboard>>($"/v1/game/events/leaderboard", "data.leaderboard",
                new { event_id, leaderboard_data = "point" });
        }
        
        // public async UniTask<List<ModelApiEventConfig>> GetConfig()
        // {
        //     var configs = await Get<List<ModelApiEventConfig>>("/v1/event/partners/config/available", "data");
        //     foreach (var config in configs)
        //     {
        //         if (config.NeedToCheckIn())
        //         {
        //             CheckIn(config.id).Forget();
        //         }
        //
        //         if (config.NeedToCheckEmoji())
        //         {
        //             // EmojiCheck().Forget();
        //         }
        //     }
        //
        //     Data.Configs = configs;
        //     Data.Notification();
        //
        //     return configs;
        // }

        public async UniTask<List<string>> ClaimGift(string partner_id)
        {
            return await Post<List<string>>($"/v1/event/partners/{partner_id}/gifts/claim", "data", new { });
        }

        public async UniTask<ModelApiEventCheckin> CheckIn(string partner_id)
        {
            var checkin = await Post<ModelApiEventCheckin>($"/v1/event/partners/{partner_id}/check-in", "data", new { });
            var item = Data.Checkins.Find(x => x.id == checkin.id);

            if (item == null)
            {
                checkin.id = partner_id;
                Data.Checkins.Add(checkin);
            }
            else
            {
                item.days = checkin.days;
                item.total_users_claimed = checkin.total_users_claimed;
                item.total_users_can_claim = checkin.total_users_can_claim;
                item.is_claimed = checkin.is_claimed;
                item.can_claim = checkin.can_claim;
            }

            return checkin;
        }
        
        public async UniTask<ModelApiClaim> CheckInClaimSign(string partner_id, string to_address)
        {
            return await Post<ModelApiClaim>($"/v1/event/partners/{partner_id}/check-in/claim/sign", "data", new { to_address });
        }

        public async UniTask<ModelApiClaimComplete> CheckInClaimComplete(string partner_id, int id, string tx_hash)
        {
            return await Post<ModelApiClaimComplete>($"/v1/event/partners/{partner_id}/check-in/claim/complete", "data", new { id, tx_hash });
        }
        
        public async UniTask<ModelApiEventJackpot> Jackpot()
        {
            var jackpot = await Get<ModelApiEventJackpot>("/v1/event/ton-jackpot", "data");
            
            // jackpot.MockUpHistory();
            
            Data.Jackpot = jackpot;
            
            Data.Notification();

            return Data.Jackpot;
        }

        public async UniTask<List<ModelApiEventJackpotMyHistory>> GetMyJackpotHistoryList()
        {
            var data= await Get<List<ModelApiEventJackpotMyHistory>>("/v1/event/ton-jackpot/my-history", "data");
            Data.JackpotHistories = data;
            return data;
        }
    }
}