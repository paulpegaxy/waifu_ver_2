using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Runtime;
using Game.Model;
using UnityEngine.Serialization;

namespace Game.UI
{
	public class BuddyReward : MonoBehaviour
	{
		[SerializeField] private Image imageBg;
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private TMP_Text textDescription;
		[SerializeField] private TMP_Text txtBerry;
		[SerializeField] private TMP_Text textTon;
		[SerializeField] private TMP_Text textLocked;
		[SerializeField] private GameObject objectLocked;
		[SerializeField] private GameObject objectAvailable;

		public void SetData(ModelBuddyCellView model)
		{
			var data = model as ModelBuddyCellViewContentNormal;

			// imageBg.sprite = ControllerSprite.Instance.GetBuddyBg(data.BuddyType);
			imageBg.LoadSpriteAutoParseAsync("buddy_bg_" + (int)data.BuddyType);

			textTitle.text = $"{Localization.Get(TextId.Buddy_Layer)} {(int)data.BuddyType}";
			textDescription.text = $"{data.Commission}% {Localization.Get(TextId.Buddy_Commission)}";
			txtBerry.text = data.HardCurrency.ToString();
			textTon.text = data.Ton.ToString();

			objectLocked.SetActive(data.IsLocked);
			objectAvailable.SetActive(!objectLocked.activeSelf);
		}
	}
}
