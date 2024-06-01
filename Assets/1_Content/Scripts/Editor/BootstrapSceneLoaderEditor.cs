using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace NJN.Scripts.Editor
{
    [InitializeOnLoad]
    public static class BootstrapSceneLoaderEditor
    {
        private const string BootstrapSceneName = "0_Bootstrap";

        static BootstrapSceneLoaderEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if (SceneManager.GetActiveScene().name != BootstrapSceneName)
                {
                    EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>($"Assets/1_Content/Scenes/Game/{BootstrapSceneName}.unity");
                }
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}