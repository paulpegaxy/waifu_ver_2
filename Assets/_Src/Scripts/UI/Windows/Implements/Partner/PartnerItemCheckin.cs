using System;
using UnityEngine;
using TMPro;
using Game.Defines;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
	public class PartnerItemCheckin : MonoBehaviour
	{
		[SerializeField] private TMP_Text textDay;
		[SerializeField] private GameObject objectClaimed;

		public void SetData(int day, PartnerCheckinStatus status)
		{
			textDay.text = day.ToString();
			objectClaimed.SetActive(status == PartnerCheckinStatus.Claimed);
		}
	}
}