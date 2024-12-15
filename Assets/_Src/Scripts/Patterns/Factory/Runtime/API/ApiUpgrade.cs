using Cysharp.Threading.Tasks;
using Game.Model;

namespace Game.Runtime
{
    [Factory(ApiType.UpgradeInfo, true)]
    public class ApiUpgrade : Api<ModelApiUpgradeInfo>
    {
        public async UniTask<ModelApiUpgradeInfo> Get()
        {
            var data = await Get<ModelApiUpgradeInfo>($"/v1/game/data", "data");
            Sync(data);
            return data;
        }

        public async UniTask PostUpgradeIdleEarn(string id)
        {
            await Post<ModelApiUpgradeInfo>($"/v1/game/upgrade-point-per-hour", "data", new { id });
            await Get();
        }
        
        public async UniTask PostUpgradePremiumWaifu(string id)
        {
            await Post<string>("/v1/game/upgrade-premium-waifu", "status", new {id});
            await Get();
        }
        
        public async UniTask PostSkipCooldown(string id,bool use_berry_to_skip)
        {
            await Post<string>("/v1/game/upgrade-premium-waifu", "status", new {id,use_berry_to_skip});
            await Get();
        }

        private void Sync(ModelApiUpgradeInfo upgradeData)
        {
            Data.upgrade = upgradeData.upgrade;
            Data.current = upgradeData.current;
            Data.next = upgradeData.next;
            Data.berry_all_time = upgradeData.berry_all_time;
            Data.point_all_time = upgradeData.point_all_time;
            Data.premium_waifu = upgradeData.premium_waifu;
            Data.Notification();
        }
    }
}