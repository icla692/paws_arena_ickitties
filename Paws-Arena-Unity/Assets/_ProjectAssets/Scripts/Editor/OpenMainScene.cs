using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class OpenMainScene: MonoBehaviour
{
    private static bool shouldPlay = false;

    static OpenMainScene()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    [MenuItem("NFT Cats/Play Game")]
    static void PlayGameFromScene()
    {
        string lastScene = EditorSceneManager.GetActiveScene().path;
        SessionState.SetString("lastScene", lastScene);
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        shouldPlay = true;
        EditorSceneManager.OpenScene("Assets/_ProjectAssets/Scenes/Lobby/Lobby.unity");
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        if (!shouldPlay) return;
        shouldPlay = false;

        EditorApplication.EnterPlaymode();
    }
    private static void OnPlayModeChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.EnteredEditMode)
        {
            string lastScene = SessionState.GetString("lastScene", "");
            if (lastScene != "")
            {
                SessionState.EraseString("lastScene");
                EditorSceneManager.OpenScene(lastScene);
            }
        }
    }
}
