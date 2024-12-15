using UnityEngine;
using TMPro;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
	public class JackpotHeader : MonoBehaviour
	{
		[SerializeField] private ItemTimer itemTimer;
		[SerializeField] private TMP_Text textPrize;

		public void SetData(ModelApiEventJackpot data)
		{
			var duration = data.reset_at - ServiceTime.CurrentUnixTime;

			itemTimer.SetDuration(duration);
			textPrize.text = $"{data.today_reward.ToDigit5()} TON";
		}
	}
}