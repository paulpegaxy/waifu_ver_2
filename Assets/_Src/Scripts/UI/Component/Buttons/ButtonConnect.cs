using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Runtime;

namespace Game.UI
{
    public class ButtonConnect : MonoBehaviour
    {
        [SerializeField] private TMP_Text textAddress;
        [SerializeField] private UIButton buttonConnect;
        [SerializeField] private UIButton buttonDisconnect;

        private void Start()
        {
            TONConnect.OnWalletChange += OnWalletChange;
            buttonConnect.onClickEvent.AddListener(OnConnect);
            buttonDisconnect.onClickEvent.AddListener(OnDisconnect);

            OnWalletChange();
        }

        private void OnDestroy()
        {
            TONConnect.OnWalletChange -= OnWalletChange;
            buttonConnect.onClickEvent.RemoveListener(OnConnect);
            buttonDisconnect.onClickEvent.RemoveListener(OnDisconnect);
        }

        private void OnConnect()
        {
            TONConnect.ConnectWallet(status => UnityEngine.Debug.Log("Connect" + status));
        }

        private void OnDisconnect()
        {
            ControllerPopup.ShowWarning(
                message: Localization.Get(TextId.Confirm_WalletDisconnect),
                ok: Localization.Get(TextId.Common_Disconnect),
                onOk: popup =>
                {
                    TONConnect.Disconnect(status =>
                    {
                        popup.Hide();
                        UnityEngine.Debug.Log("Disconnect" + status);
                    });
                }
            );
        }

        private void OnWalletChange()
        {
            var isConnected = TONConnect.IsConnected;
            if (isConnected) textAddress.text = TONConnect.Wallet.account.userFriendlyAddress.ToShortAddress();

            buttonConnect.gameObject.SetActive(!isConnected);
            buttonDisconnect.gameObject.SetActive(isConnected);
        }
    }
}