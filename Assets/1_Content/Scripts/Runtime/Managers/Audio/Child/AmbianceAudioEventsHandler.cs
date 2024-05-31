using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Fmod;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Ambiance
{
    public class AmbianceAudioEventsHandler
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public AmbianceAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;
        }

        public void SubscribeSignals()
        {
            // Other signals
        }
        
        public void Dispose()
        {
            ReleaseAllInstances();
            // Other signals
        }

        private void ReleaseAllInstances()
        {
            //releaseallinstances
        }

        // Audio events

    }
}