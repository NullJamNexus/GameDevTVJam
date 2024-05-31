using FMODUnity;
using NJN.Runtime.Components;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Player
{
    public class AudioEventsHandler 
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        
        public AudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
        }

        public void SubscribeSignals()
        {
            _signalBus.Subscribe<PlayerHideSignal>(OnHide);

            // Other signals
        }
        
        public void UnsubscribeSignals()
        {
            _signalBus.TryUnsubscribe<PlayerHideSignal>(OnHide);
            _signalBus.TryUnsubscribe<ResourceCollectedSignal>(Collect);
            // Other signals
        }

        // Audio events
        private void OnHide(PlayerHideSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.Hide);
        }

        private void Collect(ResourceCollectedSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.GatherResource);
        }
    }
}