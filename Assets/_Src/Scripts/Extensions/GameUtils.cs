using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Template.Defines;
using UnityEngine;

public static class GameUtils
{
    public static void CopyToClipboard(this string textToCopy)
    {
        var editor = new TextEditor {text = textToCopy};
        editor.SelectAll();
        editor.Copy();
    }
    
    public static string DeviceID
    {
        get
        {
#if UNITY_EDITOR
            //test
            // return SystemInfo.deviceUniqueIdentifier +
            //        Path.GetFileName(Directory.GetCurrentDirectory()) +
            //        Random.Range(1, 10000);
            return SystemInfo.deviceUniqueIdentifier + Path.GetFileName(Directory.GetCurrentDirectory());
#endif
            return SystemInfo.deviceUniqueIdentifier;
        }
    }
    
    public static void OpenLink(string url)
    {
        TelegramWebApp.OpenLink(url);
    }

    public static Color GetColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }

    public static string Parse(string text)
    {
        if (text.StartsWith("{"))
        {
            return text;
        }

        var data = text.Split(':');
        var iv = data[0].ToBytes();
        var encryptedText = data[1].ToBytes();
        var aes = Aes.Create();

#if PRODUCTION_BUILD
        var key = new byte[]
        {
            0x57, 0x74, 0x36, 0x66, 0x41, 0x54, 0x46, 0x44, 0x6D, 0x57, 0x35, 0x4C, 0x6C, 0x72, 0x59, 0x68, 0x4A, 0x79,
            0x4C, 0x7A, 0x6C, 0x74, 0x54, 0x6C, 0x46, 0x51, 0x74, 0x57, 0x4D, 0x54, 0x66, 0x32,
        };
#else
        var key = new byte[]
        {
            0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,
            0x39,0x30,0x31,0x32,0x33,0x34,0x35,0x36,
            0x37,0x38,0x39,0x30,0x31,0x32,0x33,0x34,
            0x35,0x36,0x37,0x38,0x39,0x30,0x31,0x32
        };
#endif

        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var decryptedBytes = decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
        var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        return decryptedText;
    }
    
    public static string GetText(object data)
    {
        var jObj = JObject.FromObject(data);
        jObj.Add("time", ServiceTime.CurrentUnixTime);

        var text = jObj.ToString();

#if PRODUCTION_BUILD
        var key = new byte[]
        {
            0x57, 0x74, 0x36, 0x66, 0x41, 0x54, 0x46, 0x44, 0x6D, 0x57, 0x35, 0x4C, 0x6C, 0x72, 0x59, 0x68, 0x4A, 0x79,
            0x4C, 0x7A, 0x6C, 0x74, 0x54, 0x6C, 0x46, 0x51, 0x74, 0x57, 0x4D, 0x54, 0x66, 0x32,
        };
#else
        var key = new byte[]
        {
            0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,
            0x39,0x30,0x31,0x32,0x33,0x34,0x35,0x36,
            0x37,0x38,0x39,0x30,0x31,0x32,0x33,0x34,
            0x35,0x36,0x37,0x38,0x39,0x30,0x31,0x32
        };
#endif

        var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var ms = new MemoryStream();
        var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

        using (var sw = new StreamWriter(cs))
        {
            sw.Write(text);
        }

        return BitConverter.ToString(aes.IV).Replace("-", "") + ":" + BitConverter.ToString(ms.ToArray()).Replace("-", "");
    }
    
    public static int GetPackageIndexByType(TypeShopItem type)
    {
        var mapping = new Dictionary<TypeShopItem, int>
        {
            {TypeShopItem.Daily1, 1},
            {TypeShopItem.Daily2, 2},
            {TypeShopItem.Daily21, 2},
            {TypeShopItem.Daily3, 3},
            {TypeShopItem.Daily31, 3},
            {TypeShopItem.Daily4, 4},
            {TypeShopItem.Daily5, 5},
            {TypeShopItem.Weekly1, 6},
            {TypeShopItem.Weekly2, 7},
        };

        if (!mapping.ContainsKey(type)) return 0;
        return mapping[type];
    }
    
    public static void ShuffleList<T>(ref List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);

            // Swap the elements
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    
    public static void Log(string color, string message)
    {
#if UNITY_EDITOR
        Debug.Log($"<color={color}>{message}</color>");
#endif
    }
    
    public static Color GetPartnerPackageColorBg(int index)
    {
        var colors = new List<Color>
        {
            GetColor("#B0D4E5"),
            GetColor("#BCB0E5"),
            GetColor("#E5E1B0"),
        };

        if (index < 0 || index >= colors.Count) return Color.white;
        return colors[index];
    }

    public static Color GetPartnerPackageColorTitle(int index)
    {
        var colors = new List<Color>
        {
            GetColor("#5F8192"),
            GetColor("#635984"),
            GetColor("#947149"),
        };

        if (index < 0 || index >= colors.Count) return Color.white;
        return colors[index];
    }
}
