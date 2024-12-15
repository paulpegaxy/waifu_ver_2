using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.UI;
using Template.Defines;
using Unity.VisualScripting;

namespace Game.Runtime
{
	public static class ControllerPopup
	{
		private static UIPopup _apiLoading;
		private static UIPopup _toast;
		private static UIPopup _merge;
		private static UIPopup _tutorial;
		private static UIPopup _protectInput;

		public static void SetApiLoading(bool isShow)
		{
			if (_apiLoading == null)
			{
				_apiLoading = UIPopup.Get(UIId.UIPopupName.PopupApiLoading.ToString());
			}
		
			if (isShow)
			{
				_apiLoading.InstantShow();
			}
			else
			{
				_apiLoading.InstantHide();
			}
		}
		
		public static void SetProtectInput(bool isShow)
		{
			if (_protectInput == null)
			{
				_protectInput = UIPopup.Get(UIId.UIPopupName.PopupProtectInput.ToString());
			}
		
			if (isShow)
			{
				_protectInput.InstantShow();
			}
			else
			{
				_protectInput.InstantHide();
			}
		}

		public static void HideProtectInput()
		{
			if (_protectInput == null || !_protectInput.gameObject.activeSelf)
				return;
			_protectInput.InstantHide();
		}
		
		public static void ShowToast(string message, TypeToastStatus status = TypeToastStatus.None)
		{
			if (_toast == null)
			{
				_toast = UIPopup.Get(UIId.UIPopupName.PopupToast.ToString());
			}

			_toast.GetComponent<PopupToast>().SetData(message, status);
			_toast.Show();
		}

		public static async void ShowToastError(string message)
		{
			ShowToast(message, TypeToastStatus.Error);
			await UniTask.Delay(200);
			ControllerAudio.Instance.PlaySfx(AnR.AudioKey.Fail);
		}
		
		public static async void ShowToastSuccess(string message)
		{
			ShowToast(message, TypeToastStatus.Success);
			await UniTask.Delay(200);
			ControllerAudio.Instance.PlaySfx(AnR.AudioKey.Sucess);
		}

		public static void ShowToastComingSoon()
		{
			ShowToast(Localization.Get(TextId.Common_ComingSoon));
		}
		
		public static void ShowToast(TextId textId)
		{
			ShowToast(Localization.Get(textId));
		}
	
		public static PopupTutorial ShowTutorial(ModelTutorialStep step, TutorialData data = null)
		{
			if (_tutorial == null)
			{
				_tutorial = UIPopup.Get(UIId.UIPopupName.PopupTutorial.ToString());
			}
		
			var popupTutorial = _tutorial.GetComponent<PopupTutorial>();
			popupTutorial.SetData(step, data);
		
			_tutorial.Show();
		
			return popupTutorial;
		}
		
		public static void HideTutorial()
		{
			if (_tutorial != null)
			{
				_tutorial.Hide();
			}
		}
		
		public static void ShowConfirm(string message, string ok = null, string cancel = null, Action<UIPopup> onOk = null, Action<UIPopup> onCancel = null)
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirm.ToString());
			popup.GetComponent<PopupConfirm>().SetData(message, ok, cancel, onOk, onCancel);
			popup.Show();
		}
		
		public static void ShowWarning(string message, string ok = null, string cancel = null, Action<UIPopup> onOk = null, Action<UIPopup> onCancel = null)
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupWarning.ToString());
			popup.GetComponent<PopupWarning>().SetData(message, ok, cancel, onOk, onCancel);
			popup.Show();
		}
		
		public static void ShowConfirmPurchase(string description, ModelResource resource, Action<ModelResource,UIPopup> onConfirm)
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirmPurchase.ToString());
			popup.GetComponent<PopupConfirmPurchase>().SetData(description, resource, onConfirm);
			popup.Show();
		}

		public static void ShowConfirmAskQuestX(Action<UIPopup> onConfirm)
		{
			string description = Localization.Get(TextId.Toast_DesQuestXNoti);
			var popup = UIPopup.Get(UIId.UIPopupName.PopupConfirm.ToString());
			popup.GetComponent<PopupConfirm>().SetData(description, onOk: onConfirm);
			UIPopup.AddPopupToQueue(popup);
		}
		
		public static void ShowInformation(string description)
		{
			var popup = UIPopup.Get(UIId.UIPopupName.PopupInformation.ToString());
			popup.GetComponent<PopupInformation>().SetData(description);
			popup.Show();
		}
		
		
		public static bool IsAnyPopupVisible()
		{
			if (UIPopup.visiblePopups.Count() > 0)
			{
				var name = UIPopup.visiblePopups.First().name.Replace("(Clone)", "");
				return !(name == UIId.UIPopupName.PopupToast.ToString() ||
						(name == UIId.UIPopupName.PopupTutorial.ToString() && !TutorialMgr.Instance.HasOverlay(TutorialCategory.Main)));
			}
		
			return false;
		}
	}
}