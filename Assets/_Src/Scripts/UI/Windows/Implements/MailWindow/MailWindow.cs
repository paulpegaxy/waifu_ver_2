using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Game.Runtime;
using Game.Model;
using Cysharp.Threading.Tasks;
using Template.Runtime;
using UnityEngine.SocialPlatforms;

namespace Game.UI
{
	public class MailWindow : UIWindow
	{
		[SerializeField] private MailScroller scroller;
		[SerializeField] private UIButton buttonDeleteRead;
		[SerializeField] private UIButton buttonClaimAll;
		[SerializeField] private GameObject objectMail;
		[SerializeField] private GameObject objectMailEmpty;

		private Dictionary<int, ModelApiMailData> _mails = new();

		protected override void OnEnabled()
		{
			buttonDeleteRead.onClickEvent.AddListener(OnDeleteRead);
			buttonClaimAll.onClickEvent.AddListener(OnClaimAll);
			ModelApiMail.OnChanged += OnDataChanged;

			Fetch().Forget();
		}

		protected override void OnDisabled()
		{
			buttonDeleteRead.onClickEvent.RemoveListener(OnDeleteRead);
			buttonClaimAll.onClickEvent.RemoveListener(OnClaimAll);
			ModelApiMail.OnChanged -= OnDataChanged;
		}

		private void OnDeleteRead()
		{
			ControllerPopup.ShowConfirm(
				message: Localization.Get(TextId.Mail_AskDelete),
				onOk: async (popup) =>
				{
					this.ShowProcessing();
					try
					{
						var apiMail = FactoryApi.Get<ApiMail>();
						await apiMail.DeleteAllRead();
						await Fetch();

						popup.Hide();
						this.HideProcessing();
						ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Mail_NotiReadDelete));
					}
					catch (Exception e)
					{
						e.ShowError();
					}
				}
			);
		}

		private async void OnClaimAll()
		{
            this.ShowProcessing();
			try
			{
				var apiMail = FactoryApi.Get<ApiMail>();
				var data = await apiMail.ClaimAll();
				await FactoryApi.Get<ApiGame>().GetInfo();

				foreach (var id in data.ids)
				{
					if (_mails.ContainsKey(id))
					{
						_mails[id].is_read = true;
						_mails[id].is_claimed = true;
					}
				}

				foreach (var reward in data.rewards)
				{
					ControllerResource.Add(reward.IdResource, reward.QuantityParse);
					ControllerUI.Instance.Spawn(reward.IdResource, transform.position, 20);
				}

				apiMail.Data.Notification();
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			
		}

		private void OnDataChanged(ModelApiMail data)
		{
			var currentData = scroller.GetData();
			if (currentData == null || data.Mails.Count == currentData.Count - 1)
			{
				scroller.ReloadData();
			}
			else
			{
				Fetch(false).Forget();
			}

			_mails.Clear();
			foreach (var mail in data.Mails)
			{
				_mails.Add(mail.id, mail);
			}
		}

		private async UniTask Fetch(bool isForce = true)
		{
			this.ShowProcessing();
			try
			{
				var apiMail = FactoryApi.Get<ApiMail>();
				var mails = apiMail.Data.Mails;

				if (isForce)
				{
					var mailList = await apiMail.Get();
					mails = mailList.data;
				}

				Process(mails);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private void Process(List<ModelApiMailData> mails)
		{
			objectMail.SetActive(mails.Count > 0);
			objectMailEmpty.SetActive(mails.Count == 0);

			if (!objectMail.activeSelf) return;

			var data = new List<ModelMailCellView>()
			{
				new ModelMailCellViewHeader() {},
			};

			for (var i = 0; i < mails.Count; i++)
			{
				data.Add(new ModelMailCellViewContent() { Mail = mails[i] });
			}
			scroller.SetData(data);
		}
	}
}