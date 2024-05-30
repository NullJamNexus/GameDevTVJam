using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Fmod;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Player
{
    public class PlayerAudioEventsHandler 
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public PlayerAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;
        }

        public void SubscribeSignals()
        {
            _signalBus.Subscribe<PlayerHideSignal>(OnHide);
            // Other signals
        }
        
        public void Dispose()
        {
            ReleaseAllInstances();
            _signalBus.TryUnsubscribe<PlayerHideSignal>(OnHide);
            // Other signals
        }

        private void ReleaseAllInstances()
        {
            //releaseallinstances
        }

        // Audio events
        private void OnHide(PlayerHideSignal signal)
        {
            _com.PlayOneShot(_data.Hide);
        }
    }
}