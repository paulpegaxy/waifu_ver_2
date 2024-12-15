#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenSceneEditor : Editor
{

    [MenuItem("Project/Open Scene/Startup Scene %&q", false, 0)]
    public static void OpenInit()
    {
        OpenScene("Startup");
    }

    [MenuItem("Project/Open Scene/Lobby Scene %&w", false, 1)]
    public static void OpenLobby()
    {
        OpenScene("Lobby");
    }

    [MenuItem("Project/Open Scene/Gameplay Scene %&e", false, 2)]
    public static void OpenGamePlay()
    {
        // OpenScene("Gameplay");
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/_Src/Scenes/" + sceneName + ".unity");
        }
    }
}

#endif