using Cysharp.Threading.Tasks;
using UnityEngine;
using Game.Model;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;

namespace Game.UI
{
	public class PartnerCellViewBanner : ESCellView<ModelPartnerCellView>
	{
		[SerializeField] private Image imageBackground;
		[SerializeField] private UIButton buttonInfo;

		private string _id;

		private void OnEnable()
		{
			buttonInfo.onClickEvent.AddListener(OnInfo);
		}

		private void OnDisable()
		{
			buttonInfo.onClickEvent.RemoveListener(OnInfo);
		}

		private void OnInfo()
		{
			ControllerPopup.ShowInformation(GetDescription());
		}

		public override void SetData(ModelPartnerCellView model)
		{
			var data = model as ModelPartnerCellViewBanner;
			imageBackground.LoadSpriteAsync($"header_{data.Config.id}").Forget();

			_id = data.Config.id;
			buttonInfo.gameObject.SetActive(IsShowInfo());
		}

		private bool IsShowInfo()
		{
			return _id == "emoji";
		}

		private string GetDescription()
		{
			return _id switch
			{
				"emoji" => "Emoji Description",
				_ => string.Empty,
			};
		}
	}
}