#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;





public class DBMWindow : OdinEditorWindow
{

    private DBM _dbm;
    [InlineEditor(InlineEditorObjectFieldModes.Hidden), ShowInInspector]
    private DBM Dbm
    {
        get=>_dbm ??= Resources.Load<DBM>(nameof(DBM));
        set => _dbm = value;
    } 
    

    [MenuItem("Window/DBM")]
    public static void Show()
    {

        ((EditorWindow)GetWindow<DBMWindow>()).Show();

    }

}


#endif