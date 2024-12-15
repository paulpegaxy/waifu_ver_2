using UnityEngine;
using TMPro;

namespace Game.UI
{
	public class ClubBoost : MonoBehaviour
	{
		[SerializeField] private TMP_Text textPercent;

		public void SetData(float percent)
		{
			var realPercent = Mathf.RoundToInt(percent * 100) - 100;
			textPercent.text = realPercent + "%";
		}
	}

}
