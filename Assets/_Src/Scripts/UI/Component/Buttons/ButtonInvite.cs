using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;

namespace Game.UI
{
    [RequireComponent(typeof(UIButton))]
    public class ButtonInvite : MonoBehaviour
    {
        private UIButton _buttonInvite;

        private void Awake()
        {
            _buttonInvite = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            _buttonInvite.onClickEvent.AddListener(OnInvite);
        }

        private void OnDisable()
        {
            _buttonInvite.onClickEvent.RemoveListener(OnInvite);
        }

        private void OnInvite()
        {
            SpecialExtensionGame.Invite();
        }
    }
}