using System;
using NJN.Runtime.Managers.Scenes;
using NJN.Runtime.Managers.Signals;
using NJN.Scriptables;
using UnityEngine.SceneManagement;
using Zenject;

namespace NJN.Runtime.Managers
{
    
    public class GameManager : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private SceneLoader _sceneLoader;
        
        public GameManager(SignalBus signalBus, SceneLoader sceneLoader)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<BootstrapperInitializedSignal>(OnBootstrapperInitialized);
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<BootstrapperInitializedSignal>(OnBootstrapperInitialized);
        }
        
        private void OnBootstrapperInitialized()
        {
            //_sceneLoader.LoadSceneAsync(SceneType.MainMenu, false);
            SceneManager.LoadScene("2_MainMenu", LoadSceneMode.Single);
        }
        
        private void OnMainMenuPlay()
        {
            _sceneLoader.LoadSceneAsync(SceneType.Level);
        }
        
        private void OnGameOver()
        {
            _sceneLoader.LoadSceneAsync(SceneType.MainMenu);
        }
    }
}