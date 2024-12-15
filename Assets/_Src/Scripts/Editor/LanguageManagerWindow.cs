#if UNITY_EDITOR
using I2.Loc;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class LanguageManagerWindow : OdinEditorWindow
{
    private LanguageSourceAsset _languageAsset;
    [InlineEditor(InlineEditorObjectFieldModes.Hidden), ShowInInspector]
    private LanguageSourceAsset LanguageAsset
    {
        get => _languageAsset ??= Resources.Load<LanguageSourceAsset>("I2Languages");
        set => _languageAsset = value;
    }

    [MenuItem("Window/Language Manager")]
    public static void Show()
    {
        ((EditorWindow)GetWindow<LanguageManagerWindow>()).Show();
    }
}
#endif