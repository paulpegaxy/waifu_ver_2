using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public partial class Config
{
    [field: SerializeField] public VisualConfigSOData visualConfig;
}

// [CreateAssetMenu(fileName = "VisualConfigSOData", menuName = "SO/VisualConfigSOData", order = 0)]
[Serializable]
public class VisualConfigSOData
{
    public GameMaterialConfig materialConfig;
    public GenericDictionary<int, Color> dictWaifuAvaHolderColor;
    public GenericDictionary<TypeFontMaterialName,Material> dictFontMaterialConfig;

    [SerializeField] private GenericDictionary<TypeColor, Color> dictColorText;

    public Color GetColorStatus(TypeColor type)
    {
        if (dictColorText.TryGetValue(type, out Color color))
            return color;
        
        return dictColorText[TypeColor.WHITE];
    }
    
    public Material GetMaterialFont(TypeFontMaterialName type)
    {
        if (dictFontMaterialConfig.TryGetValue(type, out Material material))
            return material;
        
        return null;
    }
    
    public Color GetWaifuAvaHolderColor(int index)
    {
        if (dictWaifuAvaHolderColor.TryGetValue(index, out Color color))
            return color;

        return dictWaifuAvaHolderColor[0];
    }
}

[Serializable]
public class GameMaterialConfig
{
    public Material matMainFontDefault;
    public Material matDisableObject;
    public Material matBlackObject;
}

public enum TypeFontMaterialName
{
    DEFAULT,
    CONTENT_SHADOW_WHITE,
    CONTENT_SHADOW_BLACK,
    CONTENT_OUTLINE_BLACK,
    CONTENT_OUTLINE_RED,
}