using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiMail : ModelApiNotification<ModelApiMail>
	{
		public List<ModelApiMailData> Mails;

		public void Claim(ModelApiMailData data)
		{
			var mail = Mails.Find(x => x.id == data.id);
			if (mail != null)
			{
				mail.is_claimed = true;
				Notification();
			}
		}

		public void Delete(ModelApiMailData data)
		{
			var mail = Mails.Find(x => x.id == data.id);
			if (mail != null)
			{
				Mails.Remove(mail);
				Notification();
			}
		}

		public override void Notification()
		{
			OnChanged?.Invoke(this);
		}
	}
}