using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using UnityEngine;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class ShopWindow : UIWindow
	{
		[SerializeField] private GameObject objContent;
		[SerializeField] private ShopPanelPal shopPanelPal;
		[SerializeField] private ShopPanelPeriod shopPanelPeriod;

		private bool _isStillHaveTutorial;
		
		protected override void OnEnabled()
		{
			// _isStillHaveTutorial = !SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.GameFeature);
			// objContent.gameObject.SetActive(!_isStillHaveTutorial);
			Fetch();
		}

		protected override void OnDisabled()
		{
			CheckAndShowIdleEarning();
		}

		private async void Fetch()
		{
			this.ShowProcessing();
			try
			{
				var api = FactoryApi.Get<ApiShop>();
				await api.Get();
				
				var apiUser = FactoryApi.Get<ApiUser>();
				// if (apiUser.Data.Subscription == null)
				// {
				// 	await apiUser.GetSubscription();
				// }

				shopPanelPal.Refresh();
		
				await UniTask.Delay(500);
				// if (_isStillHaveTutorial)
				// {
				// 	_isStillHaveTutorial = false;
					// objContent.gameObject.SetActive(true);
				// }
				OnDataLoaded?.Invoke(UIId.UIViewCategory.Window, UIId.UIViewName.Shop);
				this.HideProcessing();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}

		private void CheckAndShowIdleEarning()
		{
			var isOpenFromIdleEarning = this.GetEventData<TypeGameEvent,bool>(TypeGameEvent.ShopOpenFromIdleEarning, true);
			if (isOpenFromIdleEarning)
			{
				var apiGame = FactoryApi.Get<ApiGame>();
			
				if (apiGame.Data.IsHaveProfitFromOffline())
				{
					var popup = UIPopup.Get(UIId.UIPopupName.PopupIdleEarning.ToString());
					popup.GetComponent<PopupIdleEarning>().SetData(apiGame.Data.IdleEarning);
					popup.Show();
				}
			}
		}
	}
}