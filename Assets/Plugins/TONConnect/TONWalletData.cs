
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

using System;
using System.Collections.Generic;
[Serializable]
public class Account
{
    public string address;
    public string userFriendlyAddress;
    public string chain;
    public string walletStateInit;
    public string publicKey;
}
[Serializable]
public class Device
{
    public string platform;
    public string appName;
    public string appVersion;
    public int maxProtocolVersion;
    public List<object> features;
}
[Serializable]
public class TONWalletData
{
    public Device device;
    public string provider;
    public Account account;
    public string name;
    public string appName;
    public string imageUrl;
    public string aboutUrl;
    public string tondns;
    public List<string> platforms;
    public string bridgeUrl;
    public string universalLink;
    public string deepLink;
    public string jsBridgeKey;
    public bool injected;
    public bool embedded;
}
