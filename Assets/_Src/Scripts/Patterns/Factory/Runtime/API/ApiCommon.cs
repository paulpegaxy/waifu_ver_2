using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
    [Factory(ApiType.Common, true)]
    public class ApiCommon : Api<ModelApiCommon>
    {
        public async UniTask<ModelApiCommonServerInfo> GetServerInfo()
        {
            var data = await Get<ModelApiCommonServerInfo>("", "");

            Data.ServerInfo = data;

            return data;
        }

        public async UniTask<ModelApiCommonSummary> GetSummary()
        {
            var data = await Get<ModelApiCommonSummary>("/v1/summary", "data");



            Data.data = data;

            return data;
        }
    }
}