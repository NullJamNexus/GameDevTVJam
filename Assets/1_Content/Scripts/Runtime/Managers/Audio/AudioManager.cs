using System;
using AudioManager.Player;
using NJN.Scriptables;
using Zenject;

namespace NJN.Runtime.Managers
{
    public class AudioManager : IInitializable, IDisposable
    {
        private readonly AudioEventsHandler _audioEventsHandler;

        public AudioManager(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo)
        {
            _audioEventsHandler = new AudioEventsHandler(signalBus, audioEventBindingSo);
        }

        public void Initialize() => _audioEventsHandler.SubscribeSignals();
        public void Dispose() => _audioEventsHandler.UnsubscribeSignals();
    }
}