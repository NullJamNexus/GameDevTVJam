using FMOD.Studio;
using FMODUnity;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers;
using NJN.Runtime.Fmod;
using NJN.Scriptables;
using Zenject;

namespace AudioManager.Enemy
{
    public class EnemyAudioEventsHandler
    {
        private readonly SignalBus _signalBus;
        private readonly AudioEventBindingSO _data;
        private readonly FmodCommunication _com;
        
        public EnemyAudioEventsHandler(SignalBus signalBus, AudioEventBindingSO audioEventBindingSo, FmodCommunication fmodCommunication)
        {
            _signalBus = signalBus;
            _data = audioEventBindingSo;
            _com = fmodCommunication;
        }

        public void SubscribeSignals()
        {
            _signalBus.Subscribe<EnemyChaseSignal>(StartEnemyChase);
            _signalBus.Subscribe<EndEnemyChaseSignal>(EndEnemyChase);
        }
        
        public void Dispose()
        {
            ReleaseAllInstances();

            _signalBus.TryUnsubscribe<EnemyChaseSignal>(StartEnemyChase);
            _signalBus.TryUnsubscribe<EndEnemyChaseSignal>(EndEnemyChase);
        }

        #region Instances

        private EventInstance _chaseInstance;
        #endregion
        private void ReleaseAllInstances()
        {
            _com.RelaeseInstance(ref _chaseInstance);
        }

        private void StartEnemyChase()
        {
            _com.PlayInstanceIfNotPlaying(ref _chaseInstance ,_data.EnemyAttack);
        }

        private void EndEnemyChase()
        {
            _com.RelaeseInstance(ref _chaseInstance);
        }
    }
}