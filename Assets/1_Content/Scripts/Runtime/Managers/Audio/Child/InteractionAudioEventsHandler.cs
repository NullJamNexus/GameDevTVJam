using FMOD.Studio;
using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Fmod;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Interaction
{
    public class InteractionAudioEventsHandler
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public InteractionAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;
        }

        public void SubscribeSignals()
        {
            _signalBus.Subscribe<DoorOpenSignal>(DoorOpen);
            _signalBus.Subscribe<DoorCloseSignal>(DoorClose);
        }
        
        public void Dispose()
        {
            ReleaseAllInstances();

            _signalBus.TryUnsubscribe<DoorOpenSignal>(DoorOpen);
            _signalBus.TryUnsubscribe<DoorCloseSignal>(DoorClose);
        }

        private EventInstance _doorInstance;
        private void ReleaseAllInstances()
        {
            _com.RelaeseInstance(ref _doorInstance);
        }

        private void DoorOpen()
        {
            _com.SetInstanceAndPlay(ref _doorInstance, _data.OpenDoor);
        }

        private void DoorClose()
        {
            _com.SetInstanceAndPlay(ref _doorInstance, _data.CloseDoor);
        }

    }
}