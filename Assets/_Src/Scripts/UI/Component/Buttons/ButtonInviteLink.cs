using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;

namespace Game.UI
{
    [RequireComponent(typeof(UIButton))]
    public class ButtonInviteLink : MonoBehaviour
    {
        private UIButton _buttonCopy;

        private void Awake()
        {
            _buttonCopy = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            _buttonCopy.onClickEvent.AddListener(OnCopy);
        }

        private void OnDisable()
        {
            _buttonCopy.onClickEvent.RemoveListener(OnCopy);
        }

        private void OnCopy()
        {
            var inviteLink = SpecialExtensionGame.GetInviteReferralLink();
            TelegramWebApp.CopyToClipboard(inviteLink);
            ControllerPopup.ShowToast(Localization.Get(TextId.Toast_InviteLinkCopied));
        }
    }
}