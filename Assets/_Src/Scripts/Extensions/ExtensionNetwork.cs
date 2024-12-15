using System;
using System.Net;
using System.Net.Sockets;
using BestHTTP;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ExtensionNetwork
{
    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }

        return localIP;
    }
    
    public static void LoadFromUrl(this Image img, string url, Sprite loading = null)
    {
        img.sprite = loading;
        new HTTPRequest(new Uri(url), (req, res) =>
        {
            if (res.StatusCode is not (200 or 304)) return;
            var tex = res.DataAsTexture2D;
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2f, 100);
        }).Send();
    }
    
    public static async UniTask<bool> CheckInternetConnection()
    {
        const string echoServer = "https://google.com";
        bool result;
        using (var request = UnityWebRequest.Head(echoServer))
        {
            request.timeout = 5;
            await request.SendWebRequest();
            result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200;
        }

        if (result)
            return true;
        return false;
    }
}
