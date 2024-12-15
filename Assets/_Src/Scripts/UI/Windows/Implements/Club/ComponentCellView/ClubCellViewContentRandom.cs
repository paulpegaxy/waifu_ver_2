using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.Signals;
using TMPro;
using Game.Model;
using Game.Core;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class ClubCellViewContentRandom : ESCellView<ModelClubCellView>
	{
		[SerializeField] private ItemAvatar itemAvatar;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text txtMember;
		[SerializeField] private UIButton buttonView;

		private ModelApiClubData _data;

		private void OnEnable()
		{
			buttonView.onClickEvent.AddListener(OnView);
		}

		private void OnDisable()
		{
			buttonView.onClickEvent.RemoveListener(OnView);
		}

		private void OnView()
		{
			this.PostEvent(TypeGameEvent.ClubDetail, _data.id);
			Signal.Send(StreamId.UI.ClubDetail);
		}

		public override void SetData(ModelClubCellView model)
		{
			var data = model as ModelClubCellViewContentRandom;
			var club = data.Club;

			itemAvatar.SetNameAvatar(club.name);
			textName.text = club.name;
			txtMember.text = data.Club.total_members.ToFormat();

			_data = club;
		}
	}
}
