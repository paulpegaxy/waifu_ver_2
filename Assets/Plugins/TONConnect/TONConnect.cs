using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class TONConnectException : Exception
{
    public TONConnectException() { }
    public TONConnectException(string message) : base(message) { }

    public TONConnectException(string message, Exception innerException) : base(message, innerException) { }
}

public static class TONConnect
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void _ConnectWallet(int taskId);
    [DllImport("__Internal")]
    private static extern void _Disconnect(int taskId);
    [DllImport("__Internal")]
    private static extern string _GetWallet();
    [DllImport("__Internal")]
    private static extern void _SendTON(int taskId, string address, string amount, string payload, long validUntil);
    [DllImport("__Internal")]
    private static extern void _SendJetton(int taskId, string data);
    [DllImport("__Internal")]
    private static extern void _ClaimTON(int taskId, string data);
#endif

    public static TONWalletData Wallet { get; private set; }
    public static bool IsConnected => Wallet != null;
    public static event Action OnWalletChange;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        UpdateWallet();
    }

    public static void ConnectWallet(Action<bool> callback = null)
    {
#if UNITY_WEBGL
        if (Application.isEditor) return;
        var taskId=WebTask.Create(cb => callback?.Invoke(cb.success));
        _ConnectWallet(taskId);
#endif
    }

    public static UniTask ConnectWalletAsync()
    {
#if UNITY_WEBGL
        if (Application.isEditor)
            throw new TONConnectException("Not supported in editor");
        var tcs = new UniTaskCompletionSource<bool>();
        var taskId=WebTask.Create(cb =>
        {
            if (cb.success) tcs.TrySetResult(true);
            else tcs.TrySetException(new TONConnectException(cb.data));
        });
        _ConnectWallet(taskId);
        return tcs.Task;
#endif
        throw new TONConnectException("Not supported in this platform");
    }

    public static void Disconnect(Action<bool> callback = null)
    {
#if UNITY_WEBGL
        if (Application.isEditor) return;
        var taskId = WebTask.Create(cb => callback?.Invoke(cb.success));
        _Disconnect(taskId);
#endif
    }
    
    public static UniTask DisconnectAsync()
    {
#if UNITY_WEBGL
        if (Application.isEditor) throw new TONConnectException("Not supported in editor");
        var tcs = new UniTaskCompletionSource<bool>();
        var taskId = WebTask.Create(cb =>
        {
            if (cb.success) tcs.TrySetResult(true);
            else tcs.TrySetException(new TONConnectException(cb.data));
        });
        _Disconnect(taskId);
        return tcs.Task;
#endif
        throw new TONConnectException("Not supported in this platform");
    }
    
    public static UniTask<string> SendTonAsync(string address, decimal amount, string payload = "", long validUntil = 0)
    {
#if UNITY_WEBGL
        if (Application.isEditor) throw new TONConnectException("Not supported in editor");
        var tcs = new UniTaskCompletionSource<string>();
        var taskId = WebTask.Create(cb =>
        {
            if (cb.success)
                tcs.TrySetResult(cb.data);
            else tcs.TrySetException(new TONConnectException(cb.data));
        });
        var amountString = amount.ToString(CultureInfo.InvariantCulture);
        _SendTON(taskId, address, amountString, payload, validUntil);
        return tcs.Task;
#endif
        throw new TONConnectException("Not supported in this platform");
    }
    
    public static UniTask<string> ClaimTonAsync(string data)
    {
#if UNITY_WEBGL
        if (Application.isEditor) throw new TONConnectException("Not supported in editor");
        var tcs = new UniTaskCompletionSource<string>();
        var taskId = WebTask.Create(cb =>
        {
            if (cb.success)
                tcs.TrySetResult(cb.data);
            else tcs.TrySetException(new TONConnectException(cb.data));
        });
        _ClaimTON(taskId, data);
        return tcs.Task;
#endif
        throw new TONConnectException("Not supported in this platform");
    }
    public static UniTask<string> SendJettonAsync(string data)
    {
#if UNITY_WEBGL
        if (Application.isEditor) throw new TONConnectException("Not supported in editor");
        var tcs = new UniTaskCompletionSource<string>();
        var taskId = WebTask.Create(cb =>
        {
            if (cb.success)
                tcs.TrySetResult(cb.data);
            else tcs.TrySetException(new TONConnectException(cb.data));
        });
        _SendJetton(taskId, data);
        return tcs.Task;
#endif
        throw new TONConnectException("Not supported in this platform");
    }

    public static void UpdateWallet()
    {
#if UNITY_WEBGL
        if (Application.isEditor) return;
        var walletData = _GetWallet();
        Wallet = JsonConvert.DeserializeObject<TONWalletData>(walletData);
        OnWalletChange?.Invoke();
#endif
    }
}