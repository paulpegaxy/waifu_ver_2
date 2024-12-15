using BreakInfinity;
using UnityEngine;

public static class ExtensionColorText
{
    private static string SetColorText(this string text, string colorHex)
    {
        return $"<color=#{colorHex}>{text}</color>";
    }

    private static string ToHexString(this Color c) => $"{c.r:X2}{c.g:X2}{c.b:X2}";
    
    
    public static string SetColorText(this string text, Color color)
    {
        return text.SetColorText(color.ToHexString());
    }
    
    public static Color ToColor(this object hex)
    {
        var str = hex.ToString();
        if (!str.StartsWith("#")) str = "#" + str;
        return ColorUtility.TryParseHtmlString(str, out var color) ? color : Color.clear;
    }
    
    
    public static Color GetColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }
}
