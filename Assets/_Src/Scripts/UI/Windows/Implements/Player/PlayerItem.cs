using UnityEngine;
using TMPro;

namespace Game.UI
{
	public class PlayerItem : MonoBehaviour
	{
		[SerializeField] private TMP_Text textPlayerTitle;
		[SerializeField] private TMP_Text textPlayerCount;
		[SerializeField] private GameObject objectOnline;

		public void SetData(string title, int count, bool isOnline)
		{
			textPlayerTitle.text = title;
			textPlayerCount.text = count.ToString("#,##0");
			objectOnline.SetActive(isOnline);
		}
	}
}