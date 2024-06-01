using AudioManager.Ambiance;
using FMOD.Studio;
using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Fmod;
using NJN.Runtime.SoundSignal;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Music
{
    public class MusicAudioEventsHandler 
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public MusicAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;

            _currentMusic = EMusic.stop;
        }

        private EMusic _currentMusic;
        private EventInstance _menuInstance;
        private EventInstance _levelInstance;
        public void SubscribeSignals()
        {
            _signalBus.Subscribe<MusicSignal>(ChangeMusic);
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<MusicSignal>(ChangeMusic);

            ReleaseAllInstances();
        }

        private void ReleaseAllInstances()
        {
            _com.RelaeseInstance(ref _menuInstance);
            _com.RelaeseInstance(ref _levelInstance);
        }

        private void ChangeMusic(MusicSignal signal)
        {
            if (signal.Music == _currentMusic)
                return;

            if(_currentMusic != EMusic.stop)
                _com.RelaeseInstance(ref GetCurrentInstance());

            _currentMusic = signal.Music;
            if (_currentMusic != EMusic.stop)
                _com.SetInstanceAndPlay(ref GetCurrentInstance(), GetReferance());
        }
        private ref EventInstance GetCurrentInstance()
        {
            switch (_currentMusic)
            {
                case EMusic.menu:
                    return ref _menuInstance;
                case EMusic.level:
                    return ref _levelInstance;
                default:
                    return ref _menuInstance;
            }
        }

        private EventReference GetReferance()
        {
            switch(_currentMusic)
            {
                case EMusic.menu:
                    return _data.Mus_Menu;
                case EMusic.level:
                    return _data.Mus_Level;
                default:
                    return _data.Mus_Level;
            }
        }

    }
}