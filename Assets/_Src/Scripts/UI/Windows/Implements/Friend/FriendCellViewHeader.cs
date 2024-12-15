using UnityEngine;
using TMPro;
using Game.Model;
using UnityEngine.UI;

namespace Game.UI
{
	public class FriendCellViewHeader : ESCellView<ModelFriendCellView>
	{
		[SerializeField] private TMP_Text textTitle;
		[SerializeField] private Image imgRow;
		[SerializeField] private Color[] arrColor;

		public override void SetData(ModelFriendCellView model)
		{
			var data = model as ModelFriendCellViewHeader;
			textTitle.text = data.Title.ToUpperCase();
			imgRow.color = data.Title.ToLowerCase().Contains("list") ? arrColor[0] : arrColor[^1];
		}
	}
}
