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
        private bool _loadingSceneInitialized;
        private bool _isLoadingInProgress;
        private readonly SceneSettingsSO _sceneSettings;
        private readonly SignalBus _signalBus;
        
        private const string BOOTSTRAP_SCENE = "0_Bootstrap";
        private const string LOADING_SCENE = "1_Load";

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

            if (useLoadingScreen)
            {
                Timing.RunCoroutine(LoadSceneWithLoadingScreen(targetSceneBind.SceneName));
            }
            else
            {
                Timing.RunCoroutine(LoadScene(targetSceneBind.SceneName));
            }
        }

        private IEnumerator<float> LoadSceneWithLoadingScreen(string targetSceneName)
        {
            if (!_loadingSceneInitialized)
            {
                AsyncOperation loadingScreenOperation = InitializeLoadingScreen();
                yield return Timing.WaitUntilDone(loadingScreenOperation);
                _loadingSceneInitialized = true;
            }
            
            Scene loadingScene = SceneManager.GetSceneByName(LOADING_SCENE);
            if (loadingScene.IsValid())
            {
                SceneManager.SetActiveScene(loadingScene);
            }
            else
            {
                Debug.LogError("Loading scene is not valid.");
            }

            bool fadeInComplete = false;
            _signalBus.Fire(new LoadingFadeSignal(LoadingFadeSignal.FadeDirection.In, () => fadeInComplete = true));
            yield return Timing.WaitUntilTrue(() => fadeInComplete);

            List<AsyncOperation> unloadOperations = UnloadAllScenesExcept(new List<string> { BOOTSTRAP_SCENE, LOADING_SCENE });
            foreach (AsyncOperation unloadOperation in unloadOperations)
            {
                yield return Timing.WaitUntilDone(unloadOperation);
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                yield return Timing.WaitForOneFrame;

                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
            
            bool fadeOutComplete = false;
            _signalBus.Fire(new LoadingFadeSignal(LoadingFadeSignal.FadeDirection.Out, () => fadeOutComplete = true));
            yield return Timing.WaitUntilTrue(() => fadeOutComplete);

            Scene newlyLoadedScene = SceneManager.GetSceneByName(targetSceneName);
            if (newlyLoadedScene.IsValid())
            {
                SceneManager.SetActiveScene(newlyLoadedScene);
            }
            else
            {
                Debug.LogError("Failed to find or validate newly loaded scene.");
            }

            _isLoadingInProgress = false;
            _signalBus.Fire(new SceneLoadFinishedSignal());
        }
        
        private AsyncOperation InitializeLoadingScreen()
        {
            if (TryGetSceneBind(SceneType.Loader, out SceneBind loadingSceneBind))
                return SceneManager.LoadSceneAsync(loadingSceneBind.SceneName, LoadSceneMode.Additive);
            
            Debug.LogError("[SceneLoader] Failed to initialize loading screen. Check SceneSettings ScriptableObject...");
            return null;
        }

        private IEnumerator<float> LoadScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return Timing.WaitUntilDone(asyncLoad);

            Scene newlyLoadedScene = SceneManager.GetSceneByName(sceneName);
            if (newlyLoadedScene.IsValid())
            {
                SceneManager.SetActiveScene(newlyLoadedScene);
            }
            else
            {
                Debug.LogError("Failed to find or validate newly loaded scene.");
            }

            _isLoadingInProgress = false;
            _signalBus.Fire(new SceneLoadFinishedSignal());
        }

        private List<AsyncOperation> UnloadAllScenesExcept(List<string> sceneNamesToKeep)
        {
            List<AsyncOperation> unloadOperations = new List<AsyncOperation>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!sceneNamesToKeep.Contains(scene.name))
                {
                    AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);
                    if (unloadOperation != null)
                    {
                        unloadOperations.Add(unloadOperation);
                    }
                }
            }
            return unloadOperations;
        }
    }
}
