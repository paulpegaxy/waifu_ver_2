using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

public static class ExtensionCurrency
{
    public static string FormatTokenSemicolon<T>(this T t)
    {
        if (t.ToString().Length < 4) return t.ToString();
        var rs = "";
        var tString = t.ToString();
        var count = t.ToString().Length;
        {
            for (var i = count - 1; i > 0; i--)
                if ((count - i) % 3 != 0)
                    rs = tString[i] + rs;
                else
                    rs = "," + tString[i] + rs;
        }
        return tString[0] + rs;
    }

    public static decimal ParseDecimalNumber<T>(T number)
    {
        decimal result = 0;
        decimal.TryParse(number.ToString(), NumberStyles.Any, default, out result);
        return result;
    }

    // public static string FormatTokenAuto(string t, bool isRoundTwoPoint = false, bool isSpace = true)
    // {
    //     var splitNum = ParseDecimalNumber(t).ToString().Split('.', ',');
    //     var num = double.Parse(splitNum[0]);
    //     string newNum = "";
    //
    //     if (num < 10000)
    //     {
    //         newNum = FormatTokenSemicolon(splitNum[0]);
    //         if (splitNum.Length > 1 && isRoundTwoPoint)
    //         {
    //             if (splitNum[1].Length > 2)
    //                 newNum += "." + splitNum[1].Substring(0, 2);
    //             else
    //                 newNum += "." + splitNum[1];
    //         }
    //
    //         return newNum;
    //     }
    //
    //     if (num < 10000)
    //     {
    //         newNum = $"{num / 1000:0}";
    //         return string.Format("{0}<color=yellow>{1}</color>", FormatTokenSemicolon(newNum), (isSpace ? " K" : "K"));
    //     }
    //
    //     if (num > 1000000)
    //     {
    //         newNum = $"{num / 1000000:0}";
    //         return string.Format("{0}<color=yellow>{1}</color>", FormatTokenSemicolon(newNum), (isSpace ? " M" : "M"));
    //     }
    //
    //     // if (num > 100000000000)
    //     // {
    //     //     newNum = $"{num / 1000000000:0}";
    //     //     return string.Format("{0}<color=yellow>{1}</color>", FormatTokenSemicolon(newNum), (isSpace ? " B" : "B"));
    //     // }
    //
    //     return "";
    // }

    public static IEnumerator CorouChangeNumber(TMP_Text txt, int fromNum, int toNum, float tweenTime = 3,
        float scaleNum = 1.5f, float delay = 0)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);
        var i = 0.0f;
        var rate = 2.0f / tweenTime;
        txt.transform.DOScale(scaleNum, tweenTime);
        while (i < tweenTime)
        {
            i += Time.deltaTime * rate;
            var a = Mathf.Lerp(fromNum, toNum, i);

            txt.text = a > 0 ? string.Format("{0:0,0}", a) : "0";
            if (a == toNum) i = tweenTime;
            yield return null;
        }

        //txt.transform.localScale = Vector2.one;
        yield return new WaitForSeconds(.05f);
    }
}
