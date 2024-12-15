using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionImage
{
    public static async UniTask LoadSpriteAsync(this Image image, string key, bool nativeSize = false, Action<Sprite> callback = null)
    {
        _ = image.DOFade(0, 0);
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, 0);

        var sprite = await AnR.GetAsync<Sprite>(key);

        if (sprite != null)
        {
            image.sprite = sprite;
            if (nativeSize)
            {
                image.SetNativeSize();
            }
        }
        image.color = new Color(color.r, color.g, color.b, 1);

        callback?.Invoke(sprite);
        _ = image.DOFade(1, 0f);
    }

    public static async void LoadSpriteAutoParseAsync(this Image image, string key, bool nativeSize = false, Action<Sprite> callback = null)
    {
        var keyParse = (AnR.SpriteKey)Enum.Parse(typeof(AnR.SpriteKey), key.SnakeToPascal());
        await image.LoadSpriteAsync(AnR.GetKey(keyParse), nativeSize, callback);
    }

    public static void LoadShopIconHc(Image image, int id)
    {
        if (id > 6) id = 6;
        image.LoadSpriteAutoParseAsync("shop_berry_" + id);
    }

    public static async UniTask LoadShopTimelapse(Image image, string index="")
    {
        if (string.IsNullOrEmpty(index))
        {
            image.LoadSpriteAutoParseAsync("timelapse_1");
            return;
        }
        
        image.LoadSpriteAutoParseAsync("timelapse_" + index);
        // image.SetNativeSize();
    }

    public static void LoadRankIcon(Image image, int index, bool isCircle = false)
    {
        if (index > 4) index = 4;
        if (index < 1) index = 1;

        string key = $"rank_bg_{(isCircle ? "circle_" : "")}{index}";
        image.LoadSpriteAutoParseAsync(key);
    }
}