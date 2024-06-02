using System;
using NJN.Runtime.Managers.Scenes;
using NJN.Runtime.Managers.Signals;
using NJN.Scriptables;
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
            _signalBus.Subscribe<PlayPressedSignal>(OnPlayGame);
            _signalBus.Subscribe<GameLostSignal>(OnGameLost);
            _signalBus.Subscribe<GameWonSignal>(OnGameWon);
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<BootstrapperInitializedSignal>(OnBootstrapperInitialized);
            _signalBus.TryUnsubscribe<PlayPressedSignal>(OnPlayGame);
            _signalBus.TryUnsubscribe<GameLostSignal>(OnGameLost);
            _signalBus.TryUnsubscribe<GameWonSignal>(OnGameWon);
        }
        
        private void OnBootstrapperInitialized(BootstrapperInitializedSignal signal)
        {
            _sceneLoader.LoadSceneAsync(SceneType.MainMenu);
        }
        
        private void OnPlayGame()
        {
            _sceneLoader.LoadSceneAsync(SceneType.Level);
        }
        
        private void OnGameLost()
        {
            _sceneLoader.LoadSceneAsync(SceneType.MainMenu);
        }
        
        private void OnGameWon()
        {
            _sceneLoader.LoadSceneAsync(SceneType.MainMenu);
        }
    }
}