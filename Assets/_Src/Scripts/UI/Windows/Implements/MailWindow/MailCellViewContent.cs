using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Cysharp.Threading.Tasks;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class MailCellViewContent : ESCellView<ModelMailCellView>
	{
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private TMP_Text textDate;
		[SerializeField] private TMP_Text textDescription;
		[SerializeField] private UIButton buttonOpen;
		[SerializeField] private GameObject objectRead;
		[SerializeField] private GameObject objectUnreal;
		[SerializeField] private GameObject objectRewards;
		[SerializeField] private List<ItemReward> itemRewards;

		private ModelApiMailData _data;

		private void OnEnable()
		{
			buttonOpen.onClickEvent.AddListener(OnOpen);
		}

		private void OnDisable()
		{
			buttonOpen.onClickEvent.RemoveListener(OnOpen);
		}

		private async void OnOpen()
		{
			if (_data.rewards.Count > 0)
			{
				var popup = UIPopup.Get(UIId.UIPopupName.PopupMailInformation.ToString());
				popup.GetComponent<PopupMailInformation>().SetData(_data);
				popup.Show();
			}
			else
			{
				ControllerPopup.ShowInformation(_data.description);
			}

			if (!_data.is_read)
			{
				var apiMail = FactoryApi.Get<ApiMail>();
				await apiMail.Read(_data.id);

				_data.is_read = true;
				apiMail.Data.Notification();
			}
		}

		public override void SetData(ModelMailCellView model)
		{
			var data = model as ModelMailCellViewContent;
			var mail = data.Mail;
			mail.rewards ??= new List<ModelApiItemData>();

			textTitle.text = mail.title;
			textDate.text = mail.createdAt.ToShortDateString();
			textDescription.text = mail.description;

			objectRead.SetActive(mail.is_read);
			objectUnreal.SetActive(!mail.is_read);
			objectRewards.SetActive(mail.rewards.Count > 0);
			textDescription.gameObject.SetActive(!objectRewards.activeSelf);

			for (var i = 0; i < itemRewards.Count; i++)
			{
				if (i < mail.rewards.Count)
				{
					
					itemRewards[i].SetData(mail.rewards[i].IdResource, mail.rewards[i].QuantityParse);
					itemRewards[i].SetOverlay(mail.is_claimed);
				}

				itemRewards[i].gameObject.SetActive(i < mail.rewards.Count);
			}

			_data = mail;
		}
	}
}