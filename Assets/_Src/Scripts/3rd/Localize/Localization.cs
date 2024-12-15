using I2.Loc;
using Game.Runtime;
using UnityEngine;

public partial class Localization
{
    public static string Get(TextId id)
    {
        if (id == TextId.None) return string.Empty;

        string[] texts = id.ToString().Split('_');
        string key = $"{texts[0].PascalToSnake().ToUpperCase()}/{texts[1].PascalToSnake().ToUpperCase()}";

        // Debug.LogError("Key: "+key);
        
        return LocalizationManager.GetTranslation(key);
    }

    public static string Get(string key)
    {
        // Debug.LogError("Key: "+key);
        return LocalizationManager.GetTranslation(key);
    }
}