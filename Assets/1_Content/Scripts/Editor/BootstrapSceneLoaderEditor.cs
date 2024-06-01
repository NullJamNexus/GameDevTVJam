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
                Scene activeScene = SceneManager.GetActiveScene();
                if (activeScene.name != BootstrapSceneName && IsSceneInBuildSettings(activeScene.name))
                {
                    EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>($"Assets/1_Content/Scenes/Game/{BootstrapSceneName}.unity");
                }
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }

        private static bool IsSceneInBuildSettings(string sceneName)
        {
            foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
            {
                if (buildScene.path.Contains(sceneName) && buildScene.enabled)
                {
                    return true;
                }
            }
            return false;
        }
    }
}