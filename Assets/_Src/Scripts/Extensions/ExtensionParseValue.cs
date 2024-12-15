using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ExtensionParseValue
{
    public static bool NearValue(this double d1, double d2)
    {
        var padding = Mathf.Abs((float) (d1 - d2));
        return padding <= 0.00001f;
    }
    public static int ParsePercentToNum(this float percent)
    {
        return (int) (percent * 100);
    }
    
    public static float RandomWeighted(List<float> weight)
    {
        float weightTotal = 0;

        for (int i = 0; i < weight.Count; i++)
        {
            weightTotal += weight[i];
        }

        float result = 0, total = 0;
        float randVal = Random.Range(0, weightTotal);
        for (result = 0; result < weight.Count; result++)
        {
            total += weight[(int) result];
            if (total > randVal) break;
        }

        return result - 1;
    }

    public static string ParseLocalIAPPrice(decimal localizePrice, string iso)
    {
        return $"{localizePrice.ToString("N2")} {iso}";
    }
    
    public static byte[] DecodeUrlBase64(string s)
    {
        s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
        return Convert.FromBase64String(s);
    }
    
    public static long ParseVersion(this string version)
    {
        var spl = version.Split('.');
        return int.Parse(spl[0]) * 1000000 + int.Parse(spl[1]) * 1000 + int.Parse(spl[2]);
    }

    public static T Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
