using System;
using UnityEngine;
using UnityEngine.UI;

public class TONConnectButton : MonoBehaviour
{
    [SerializeField] private Button btnConnect;
    [SerializeField] private Button btnAddress;
    [SerializeField] private Button btnDisconnect;
    private void Awake()
    {
        btnConnect.onClick.AddListener(OnClickBtnConnect);
        btnAddress.onClick.AddListener(OnClickBtnAddress);
        btnDisconnect.onClick.AddListener(OnClickBtnDisconnect);
        TONConnect.OnWalletChange += UpdateUI;
        UpdateUI();
    }

    private void OnClickBtnConnect()
    {
        TONConnect.ConnectWallet(s => Debug.Log("Connect" + s));
    }
    private void OnClickBtnDisconnect()
    {
        TONConnect.Disconnect(s => Debug.Log("Connect" + s));
    }
    private void OnClickBtnAddress()
    {
        Debug.Log(TONConnect.Wallet.account.address);
    }
    private void OnDestroy()
    {
        btnConnect.onClick.RemoveListener(OnClickBtnConnect);
        btnAddress.onClick.RemoveListener(OnClickBtnAddress);
        btnDisconnect.onClick.RemoveListener(OnClickBtnDisconnect);
        TONConnect.OnWalletChange -= UpdateUI;
    }

    private void UpdateUI()
    {
        btnConnect.gameObject.SetActive(!TONConnect.IsConnected);
        btnAddress.gameObject.SetActive(TONConnect.IsConnected);
        if (TONConnect.IsConnected) btnAddress.GetComponentInChildren<Text>().text = TONConnect.Wallet.account.address;
        btnDisconnect.gameObject.SetActive(TONConnect.IsConnected);
    }
}
