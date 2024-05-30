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
            _signalBus.Subscribe<HideSignal>(OnHide);
            // Other signals
        }
        
        public void UnsubscribeSignals()
        {
            _signalBus.TryUnsubscribe<HideSignal>(OnHide);
            // Other signals
        }

        // Audio events
        private void OnHide(HideSignal signal)
        {
            RuntimeManager.PlayOneShot(_data.Hide);
        }
    }
}