using AudioManager.Player;
using NJN.Runtime.Components;
using NJN.Scriptables.Audio.Player;
using UnityEngine;
using Zenject;

namespace AudioManager
{
    public class AudioManager : BaseComponent
    {
        #region Instantiate
        private SignalBus _signalBus;
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private static AudioManager _instance;
        private void Awake()
        {
            if(_instance == null )
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Managers
        private PlayerAudioManager _playerAudioManager;
        #endregion

        #region EventData
        [SerializeField] private PlayerAudioEvents _playerAudioEvents;
        #endregion
        private void Initialize()
        {
            _playerAudioManager = new PlayerAudioManager(_signalBus, _playerAudioEvents);
        }

        private void OnDestroy()
        {
            if (_playerAudioManager == null)
                return;
            _playerAudioManager.Destroy();
            
        }
    }
}