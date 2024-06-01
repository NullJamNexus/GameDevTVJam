using System.Collections.Generic;
using NJN.Scriptables;
using MEC;
using NJN.Runtime.Managers.Scenes.Signals;
using NJN.Runtime.Scenes;
using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Managers.Scenes
{
    public class SceneLoader
    {
        private bool _isLoadingInProgress;
        private readonly SceneSettingsSO _sceneSettings;
        private SignalBus _signalBus;

        public SceneLoader(SceneSettingsSO sceneSettings, SignalBus signalBus)
        {
            _sceneSettings = sceneSettings;
            _signalBus = signalBus;
        }

        public void LoadSceneAsync(SceneType targetSceneType, bool useLoadingScreen = true)
        {
            if (IsLoadingInProgress())
                return;

            if (!TryGetSceneBind(targetSceneType, out SceneBind targetSceneBind))
                return;

            StartSceneLoad(targetSceneBind, useLoadingScreen);
        }

        private bool IsLoadingInProgress()
        {
            if (!_isLoadingInProgress) return false;

            Debug.LogError("[SceneLoader] Attempted to load a scene while another scene load is in progress.");
            return true;
        }

        private bool TryGetSceneBind(SceneType sceneType, out SceneBind sceneBind)
        {
            sceneBind = _sceneSettings.SceneBinds.Find(bind => bind.SceneType == sceneType);
            if (!string.IsNullOrEmpty(sceneBind.SceneName))
                return true;

            Debug.LogError($"[SceneLoader] SceneBind for {sceneType} not found. Check SceneSettings ScriptableObject...");
            return false;
        }

        private void StartSceneLoad(SceneBind targetSceneBind, bool useLoadingScreen)
        {
            _isLoadingInProgress = true;
            _signalBus.Fire(new SceneLoadStartedSignal());

            if (useLoadingScreen && TryGetSceneBind(SceneType.Loader, out SceneBind loadingSceneBind))
            {
                Timing.RunCoroutine(LoadSceneWithLoadingScreen(loadingSceneBind.SceneName, targetSceneBind.SceneName));
            }
            else
            {
                Timing.RunCoroutine(LoadScene(targetSceneBind.SceneName));
            }
        }

        private IEnumerator<float> LoadSceneWithLoadingScreen(string loadingSceneName, string targetSceneName)
        {
            Debug.Log("Started loading scene with loading screen...");

            // Unload unused assets before loading new scenes
            Resources.UnloadUnusedAssets();
            yield return Timing.WaitForOneFrame;

            // Load the loading screen additively
            AsyncOperation loadingSceneLoadOperation = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
            yield return Timing.WaitUntilDone(loadingSceneLoadOperation);
            Debug.Log("Loading screen scene loaded.");

            // Start loading the target scene additively but don't activate it yet
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            Debug.Log("Started loading target scene...");
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    Debug.Log("Target scene almost loaded.");
                    asyncLoad.allowSceneActivation = true;
                }

                yield return Timing.WaitForOneFrame;
            }

            Scene newlyLoadedScene = SceneManager.GetSceneByName(targetSceneName);
            if (newlyLoadedScene.IsValid())
            {
                Debug.Log("Setting active scene to newly loaded scene");
                SceneManager.SetActiveScene(newlyLoadedScene);
            }
            else
            {
                Debug.LogError("Failed to find or validate newly loaded scene.");
            }

            Debug.Log("Waiting for graphics operations to complete...");
            yield return Timing.WaitForSeconds(0.5f);

            Debug.Log("Started unloading loading screen...");
            AsyncOperation loadingSceneUnloadOperation = SceneManager.UnloadSceneAsync(loadingSceneName);
            yield return Timing.WaitUntilDone(loadingSceneUnloadOperation);
            Debug.Log("Loading screen scene unloaded.");

            // Delay to allow graphics operations to complete
            yield return Timing.WaitForSeconds(0.5f);

            // Force garbage collection to clean up resources
            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            _isLoadingInProgress = false;
            _signalBus.Fire(new SceneLoadFinishedSignal());
        }

        private IEnumerator<float> LoadScene(string sceneName)
        {
            Debug.Log($"Started loading scene: {sceneName}");

            //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            //yield return Timing.WaitUntilDone(asyncLoad);
            yield return Timing.WaitForOneFrame;
            
            _isLoadingInProgress = false;
            _signalBus.Fire(new SceneLoadFinishedSignal());
        }
    }
}
