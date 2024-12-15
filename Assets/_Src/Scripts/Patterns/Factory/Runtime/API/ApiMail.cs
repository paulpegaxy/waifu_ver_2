using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Model;

namespace Game.Runtime
{
    [Factory(ApiType.Mail, true)]
    public class ApiMail : Api<ModelApiMail>
    {
        public async UniTask<ModelApiMailList> Get(int page = 1, int take = 999)
        {
            var mailList = await Get<ModelApiMailList>("/v1/mail/me", "data", new { page, take });
            Data.Mails = mailList.data;
            Data.Notification();

            return mailList;
        }

        public async UniTask<ModelApiMailRead> Read(int id)
        {
            return await Post<ModelApiMailRead>($"/v1/mail/{id}/read", "data", new { });
        }

        public async UniTask<ModelApiMailRead> ReadAll()
        {
            return await Post<ModelApiMailRead>($"/v1/mail/read-all", "data", new { });
        }

        public async UniTask<ModelApiMailClaim> Claim(int id)
        {
            return await Post<ModelApiMailClaim>($"/v1/mail/{id}/claim", "data", new { });
        }

        public async UniTask<ModelApiMailClaim> ClaimAll()
        {
            return await Post<ModelApiMailClaim>($"/v1/mail/claim-all", "data", new { });
        }

        public async UniTask<bool> Delete(int id)
        {
            return await Post<bool>($"/v1/mail/{id}/delete", "data", new { });
        }

        public async UniTask<bool> DeleteAllRead()
        {
            return await Post<bool>($"/v1/mail/delete-all-read", "data", new { });
        }
    }
}