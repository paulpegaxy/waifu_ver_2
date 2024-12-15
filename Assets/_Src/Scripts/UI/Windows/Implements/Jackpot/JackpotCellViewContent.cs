using UnityEngine;
using TMPro;
using Game.Model;
using Game.Runtime;
using Sirenix.Utilities;
using UnityEngine.UI;

namespace Game.UI
{
	public class JackpotCellViewContent : ESCellView<AModelJackpotCellView>
	{
		[SerializeField] private Image imgStatus;
		[SerializeField] private ItemAvatar itemAvatar;
		[SerializeField] private TMP_Text textName;
		[SerializeField] private TMP_Text textDate;
		[SerializeField] private TMP_Text textPrize;
		[SerializeField] private JackpotItemTicket[] tickets;
		
		public override void SetData(AModelJackpotCellView model)
		{
			if (model is ModelJackpotCellViewContent data)
			{
				itemAvatar.SetNameAvatar(data.History.user.name);
				textName.text = data.History.user.name;
				textDate.text = data.History.created_at.ToShortDateString();
				textPrize.text = data.History.win_amount.ToDigit5();
				tickets.ForEach(x => x.Clear());

				int count = data.History.bought_tickets.Count;

				for (int i = 0; i < count; i++)
				{
					var ticket = data.History.bought_tickets[i];
					if (ticket == data.History.win_ticket)
					{
						tickets[i].SetTicketWin(ticket);
					}
					else
						tickets[i].SetData(ticket);
				}
			}
		}
	}
}