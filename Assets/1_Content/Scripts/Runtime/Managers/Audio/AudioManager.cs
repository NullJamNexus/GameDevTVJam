using System;
using AudioManager.Ambiance;
using AudioManager.Enemy;
using AudioManager.Interaction;
using AudioManager.Music;
using AudioManager.Player;
using NJN.Runtime.Fmod;
using NJN.Scriptables;
using Zenject;

namespace NJN.Runtime.Managers
{
    public class AudioManager : IInitializable, IDisposable
    {
        private readonly FmodCommunication _fmodCommunication;

        private readonly PlayerAudioEventsHandler _playerAudioEventsHandler;
        private readonly MusicAudioEventsHandler _musicAudioEventsHandler;
        private readonly AmbianceAudioEventsHandler _ambianceAudioEventsHandler;
        private readonly EnemyAudioEventsHandler _enemyAudioEventsHandler;
        private readonly InteractionAudioEventsHandler _interactionAudioEventsHandler;
        public AudioManager(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo)
        {
            _fmodCommunication = new FmodCommunication();
            _playerAudioEventsHandler = new PlayerAudioEventsHandler(signalBus, audioEventBindingSo, _fmodCommunication);
            _musicAudioEventsHandler = new MusicAudioEventsHandler(signalBus, audioEventBindingSo, _fmodCommunication);
            _ambianceAudioEventsHandler = new AmbianceAudioEventsHandler(signalBus, audioEventBindingSo, _fmodCommunication);
            _enemyAudioEventsHandler = new EnemyAudioEventsHandler(signalBus, audioEventBindingSo, _fmodCommunication);
            _interactionAudioEventsHandler = new InteractionAudioEventsHandler(signalBus, audioEventBindingSo, _fmodCommunication);
        }

        public void Initialize()
        {
            _playerAudioEventsHandler.SubscribeSignals();
            _musicAudioEventsHandler.SubscribeSignals();
            _ambianceAudioEventsHandler.SubscribeSignals();
            _enemyAudioEventsHandler.SubscribeSignals();
            _interactionAudioEventsHandler.SubscribeSignals();
        }
        public void Dispose()
        {
            _playerAudioEventsHandler.Dispose();
            _musicAudioEventsHandler.Dispose();
            _ambianceAudioEventsHandler.Dispose();
            _enemyAudioEventsHandler.Dispose();
            _interactionAudioEventsHandler.Dispose();
        }
    }
}