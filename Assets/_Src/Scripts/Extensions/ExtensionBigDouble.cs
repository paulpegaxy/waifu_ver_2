using System;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

public static class ExtensionBigDouble
{
    private static readonly List<string> _suffixs = new();

    static ExtensionBigDouble()
    {
        _suffixs = GenerateSuffixs();
    }

    public static float ToFloat(this BigDouble value)
    {
        return Convert.ToSingle(value.ToDouble());
    }

    public static string ToLetter(this BigDouble value)
    {

        int index = 0;
        // Debug.LogError($"Value: {value}");
        if (value > 10000)
        {
            while (value >= 1000)
            {
                value /= 1000;
                index++;
            }

            // Debug.Log($"Index: {index}");
            if (index > 0)
            {
                var letter = _suffixs[index];
                while (letter[0] == 'a' && letter.Length > 2)
                {
                    letter = letter.Substring(1);
                }

                return $"{value.ToDouble():.##}{letter}";
            }
        }

        return Mathf.RoundToInt((float)value.ToDouble()).ToString("#,##0");
    }

    public static string ToTime(this BigDouble value)
    {

        int totalSecond = (int)value.ToDouble();
        int hours = totalSecond / 3600;
        int minutes = totalSecond % 3600 / 60;
        int seconds = totalSecond % 60;

        if (hours > 0)
        {
            return $"{hours}h {minutes}m";
        }
        else if (minutes > 0)
        {
            return $"{minutes}m {seconds}s";
        }
        else
        {
            return $"{seconds}s";
        }
    }

    private static List<string> GenerateSuffixs()
    {
        List<string> suffixs = new() { "", "K", "M", "B", "T", "Q" };

        for (int i = 65; i < 90; i++)
        {
            for (int j = 65; j < 90; j++)
            {
                for (int k = 65; k < 90; k++)
                {
                    for (int l = 65; l < 90; l++)
                    {
                        suffixs.Add($"{(char)(i)}{(char)(j)}{(char)k}{(char)l}".ToLower());
                    }
                }
            }
        }

        return suffixs;
    }
}